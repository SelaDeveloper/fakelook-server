using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class PostRepository : IPostRepository
    {
        readonly private DataContext _context;
        readonly private IDtoConverter _dtoConverter;
        readonly private ITagRepository _tagRepository;
        readonly private IUserRepository _userRepository;
        readonly private ICommentRepository _commentRepository;

        public PostRepository(DataContext context, IDtoConverter dtoConverter,
            ITagRepository tagRepository, IUserRepository userRepository,
            ICommentRepository commentRepository)
        {
            _context = context;
            _dtoConverter = dtoConverter;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<Post> Add(Post item)
        {
            List<UserTaggedPost> taggedUserList = item.UserTaggedPost.ToList();
            // Clear the Taggs of the post
            //item.Tags.Clear();
            item.UserTaggedPost.Clear();
            // Add tags to post - tag table
            item.Tags = await AddTagsToPost(item.Tags.ToList());
            // Add tag to context
            var res = _context.Posts.Add(item);
            User u = _userRepository.GetById(item.UserId);
            item.User.UserName = u.UserName;
            foreach (var userTagged in taggedUserList)
            {

                User tempU = _userRepository.GetByUserName(userTagged.User.UserName);
                if (tempU == null) { continue; }
                //User user = new User { Id = id, UserName = _userRepository.GetById(id).UserName };
                User user = _userRepository.GetById(tempU.Id);
                res.Entity.UserTaggedPost.Add(new UserTaggedPost { UserId = tempU.Id, PostId = item.Id, User = user });
            }
            // Add User object to Post
            await _context.SaveChangesAsync();
            return res.Entity;
        }


        public async Task<Post> Delete(int id)
        {
            var post = _context.Posts.Where(post => post.Id == id).Single();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> Edit(Post item)
        {
            List<Tag> tags = new List<Tag>();
            List<Tag> tagsList = new List<Tag>();
            List<UserTaggedPost> userTaggedList = new List<UserTaggedPost>();
            tagsList = item.Tags.ToList();
            userTaggedList = item.UserTaggedPost.ToList();
            // Clear the post's tags and userTagged
            item.Tags.Clear();
            item.UserTaggedPost.Clear();
            // Clear the post's tags and userTagged from context
            var tagsC = _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.UserTaggedPost)
                .Where(p => p.Id == item.Id).SingleOrDefault();
            tagsC.Tags.Clear();
            tagsC.UserTaggedPost.Clear();
            // Add tags to post - tag table
            tags = await AddTagsToPost(tagsList);
            // Update the post without the tags 
            var res = _context.Posts.Update(tagsC);
            // Add new Taggs to post
            foreach (var tag in tags)
            {
                res.Entity.Tags.Add(tag);
            }
            // Add new userTagged to post
            foreach (var userTagged in userTaggedList)
            {
                int id = _userRepository.GetByUserName(userTagged.User.UserName).Id;
                res.Entity.UserTaggedPost.Add(new UserTaggedPost { UserId = id, PostId = item.Id });
                //res.Entity.UserTaggedPost.Add(userTagged);
            }
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public ICollection<Post> GetAll()
        {
            var posts = _context.Posts
                .OrderByDescending(d => d.Date)
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .Include(p => p.UserTaggedPost)
                .ThenInclude(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserTaggedComment)
                .ThenInclude(p => p.User)
                .Select(DtoLogic).ToList();
            return posts;
        }


        public Post GetById(int id)
        {
            var post = _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .Include(p => p.User)
                .Include(p => p.UserTaggedPost)
                .ThenInclude(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserTaggedComment)
                .ThenInclude(p => p.User)
                .Select(DtoLogic).SingleOrDefault(p => p.Id == id);
            return post;
        }

        public ICollection<Post> GetByPredicate(Func<Post, bool> predicate)
        {
            return _context.Posts.Include(p => p.Tags)
                .Include(p => p.UserTaggedPost)
                .ThenInclude(p => p.User)
                .Select(DtoLogicReduced)
                .Where(predicate).ToList();
        }

        public string ConvetUserIdToUserName(int id)
        {
            string userName = string.Empty;
            userName = _context.Users.Where(u => u.Id == id)
                .SingleOrDefault().UserName;
            return userName;
        }

        public async Task<Post> LikeUnLike(int postId, int userId)
        {
            Post post = GetById(postId);
            Like like = post.Likes.Where(l => l.PostId == postId && l.UserId == userId).
                SingleOrDefault();
            if (like == null)
            {
                _context.Posts.Where(p => p.Id == postId).SingleOrDefault().Likes
                      .Add(new Like() { UserId = userId, PostId = postId, IsActive = true });
            }
            else
            {
                bool wantedLikeValue = !like.IsActive;
                _context.Posts.Where(p => p.Id == postId).SingleOrDefault().Likes.
                    Where(l => l.UserId == userId).SingleOrDefault().IsActive = wantedLikeValue;
                //l.IsActive = !l.IsActive;
            }
            post = GetById(postId);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> AddCommentToPost(Comment item)
        {

            //return await _commentRepository.Add(comment);
            // Add Comment through the CommentRepository
            var comment = await _commentRepository.Add(item);
            // Add Comment to post
            //Post post = 
            //await _context.SaveChangesAsync();
            return GetById(item.PostId); 
        }

        private async Task<List<Tag>> AddTagsToPost(ICollection<Tag> tags)
        {
            return await _tagRepository.AddTags(tags);
        }

        private Post DtoLogicReduced(Post post)
        {
            var dtoPost = _dtoConverter.DtoPost(post);
            // User ID
            dtoPost.UserId = post.UserId;
            // Tags
            dtoPost.Tags = post.Tags?.Select(c =>
            {
                var dtoTag = _dtoConverter.DtoTag(c);
                return dtoTag;
            }).ToArray();
            // UserTaggedPost
            dtoPost.UserTaggedPost = post.UserTaggedPost?.Select(u =>
            {
                var dtoTaggedPost = _dtoConverter.DtoUserTaggedPost(u);
                dtoTaggedPost.User = _dtoConverter.DtoUser(u.User);
                return dtoTaggedPost;
            }).ToArray();

            return dtoPost;
        }

        private Post DtoLogic(Post post)
        {
            var dtoPost = _dtoConverter.DtoPost(post);
            // User
            dtoPost.User = _dtoConverter.DtoUser(post.User);
            // User ID
            dtoPost.UserId = post.UserId;
            // Comments
            dtoPost.Comments = post.Comments?.Select(c =>
            {
                var dtoComment = _dtoConverter.DtoComment(c);
                // User of the comment
                dtoComment.User = _dtoConverter.DtoUser(c.User);
                // User ID of the comment
                dtoComment.UserId = c.UserId;
                // Tags of the comment
                dtoComment.Tags = c.Tags?.Select(t =>
                {
                    var dtoCommentTag = _dtoConverter.DtoTag(t);
                    return dtoCommentTag;
                }).ToArray();
                // UserTags of the comment
                dtoComment.UserTaggedComment = c.UserTaggedComment?.Select(t =>
                {
                    var dtoUserTaggedComment = _dtoConverter.DtoUserTaggedComment(t);
                    dtoUserTaggedComment.User = _dtoConverter.DtoUser(t.User);
                    return dtoUserTaggedComment;
                }).ToArray();
                return dtoComment;
            }).ToArray();
            // Likes
            dtoPost.Likes = post.Likes?.Select(c =>
            {
                var dtoLike = _dtoConverter.DtoLike(c);
                // Like Id of like
                dtoLike.Id = c.Id;
                // User of the like
                //dtoLike.User = _dtoConverter.DtoUser(c.User);
                // IsActive of the like
                dtoLike.IsActive = c.IsActive;
                // UserId of like
                dtoLike.UserId = c.UserId;
                // PostId of like
                dtoLike.PostId = c.PostId;
                return dtoLike;
            }).ToArray();
            // Tags
            dtoPost.Tags = post.Tags?.Select(c =>
            {
                var dtoTag = _dtoConverter.DtoTag(c);
                return dtoTag;
            }).ToArray();
            // UserTaggedPost
            dtoPost.UserTaggedPost = post.UserTaggedPost?.Select(u =>
            {
                var dtoTaggedPost = _dtoConverter.DtoUserTaggedPost(u);
                dtoTaggedPost.User = _dtoConverter.DtoUser(u.User);
                return dtoTaggedPost;
            }).ToArray();

            return dtoPost;
        }


    }
}

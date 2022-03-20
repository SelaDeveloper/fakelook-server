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

        public PostRepository(DataContext context, IDtoConverter dtoConverter,
            ITagRepository tagRepository, IUserRepository userRepository)
        {
            _context = context;
            _dtoConverter = dtoConverter;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public async Task<Post> Add(Post item)
        {
            List<Tag> tags = new List<Tag>();
            List<Tag> tagsList = new List<Tag>();
            List<UserTaggedPost> taggedUserList = new List<UserTaggedPost>();
            tagsList = item.Tags.ToList();
            taggedUserList = item.UserTaggedPost.ToList();
            item.Tags.Clear();
            item.UserTaggedPost.Clear();
            tags = await AddTagsToPost(tagsList);
            var res = _context.Posts.Add(item);
            foreach (var tag in tags)
            {
                res.Entity.Tags.Add(tag);
            }
            foreach (var userTagged in taggedUserList)
            {
                int id = _userRepository.GetByUserName(userTagged.User.UserName).Id;
                res.Entity.UserTaggedPost.Add(new UserTaggedPost { UserId = id, PostId = item.Id });
            }
            //res.Entity.UserTaggedPost.Union(item.UserTaggedPost);
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
            // Update the post without the tags 
            var res = _context.Posts.Update(tagsC);
            // Add new Taggs to post
            foreach (var tag in tagsList)
            {
                res.Entity.Tags.Add(tag);
            }
            // Add new userTagged to post
            foreach (var userTagged in userTaggedList)
            {
                res.Entity.UserTaggedPost.Add(userTagged);
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
                .Include(p => p.UserTaggedPost)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserTaggedComment)
                .Select(DtoLogic).ToList();
            return posts;
        }


        public Post GetById(int id)
        {
            var post = _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Tags)
                .Include(p => p.UserTaggedPost)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Tags)
                .Include(p => p.Comments)
                .ThenInclude(c => c.UserTaggedComment)
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
                      .Append(new Like() { UserId = userId, PostId = postId, IsActive = true });
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
            dtoPost.UserId = post.UserId;
            // Comments
            dtoPost.Comments = post.Comments?.Select(c =>
            {
                var dtoComment = _dtoConverter.DtoComment(c);
                // User of the comment
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
                    return dtoUserTaggedComment;
                }).ToArray();
                return dtoComment;
            }).ToArray();
            // Likes
            dtoPost.Likes = post.Likes?.Select(c =>
            {
                var dtoLike = _dtoConverter.DtoLike(c);
                dtoLike.Id = c.Id;
                // User of the like
                dtoLike.UserId = c.UserId;
                dtoLike.PostId = c.PostId;
                // IsActive of the like
                dtoLike.IsActive = c.IsActive;
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
                return dtoTaggedPost;
            }).ToArray();

            return dtoPost;
        }


    }
}

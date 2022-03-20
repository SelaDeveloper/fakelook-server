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

        public PostRepository(DataContext context, IDtoConverter dtoConverter,
            ITagRepository tagRepository)
        {
            _context = context;
            _dtoConverter = dtoConverter;
            _tagRepository = tagRepository;
        }

        public async Task<Post> Add(Post item)
        {
            AddTagsToPost(item.Tags);
            var res = _context.Posts.Add(item);
            res.Entity.UserTaggedPost.Union(item.UserTaggedPost);
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
            var res = _context.Posts.Update(item);
            await _context.SaveChangesAsync();
            return DtoLogic(res.Entity);
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
            return _context.Posts.Where(predicate).ToList();
        }

        public async Task<Post> LikeUnLike(int postId, int userId)
        {
            Post post = GetById(postId);
            Like like = post.Likes.Where(l => l.PostId == postId && l.UserId == userId).
                SingleOrDefault();
            if(like == null)
            {
              _context.Posts.Where(p => p.Id == postId).SingleOrDefault().Likes
                    .Append(new Like() { UserId = userId , PostId = postId, IsActive = true});
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

        private void AddTagsToPost(ICollection<Tag> tags)
        {
            _tagRepository.AddTags(tags);
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

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
        public PostRepository(DataContext context, IDtoConverter dtoConverter)
        {
            _context = context;
            _dtoConverter = dtoConverter;

        }

        public async Task<Post> Add(Post item)
        {
            var res = _context.Posts.Add(item);
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
            return res.Entity;
        }

        public ICollection<Post> GetAll()
        
            {
                var posts = _context.Posts.OrderByDescending(d => d.Date)
                    .Include(p => p.Likes)
                    .Include(p => p.Tags)
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.Tags)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.UserTaggedComment)
                    .Select(DtoLogic).ToList();
                return posts;
            }
        

        public Post GetById(int id)
        {
            return _context.Posts.SingleOrDefault(p => p.Id == id);
        }

        public ICollection<Post> GetByPredicate(Func<Post,bool> predicate)
        {
            return _context.Posts.Where(predicate).ToList();
        }

        private Post DtoLogic(Post post)
        {
            var dtoPost = _dtoConverter.DtoPost(post);
            // User
            dtoPost.User = _dtoConverter.DtoUser(post.User);
            // Comments
            dtoPost.Comments = post.Comments?.Select(c =>
            {
                var dtoComment = _dtoConverter.DtoComment(c);
                // User of the comment
                dtoComment.User = _dtoConverter.DtoUser(c.User);
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
                // User of the like
                dtoLike.User = _dtoConverter.DtoUser(c.User);
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

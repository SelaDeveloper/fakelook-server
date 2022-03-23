using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        readonly IUserRepository _userRepository;
        readonly private DataContext _context;
        readonly private ITagRepository _tagRepository;
        public CommentRepository(DataContext dataContext, IUserRepository userRepository,
            ITagRepository tagRepository)
        {
            _context = dataContext;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
        }
        public async Task<Comment> Add(Comment item)
        {
            List<Tag> tags = new List<Tag>();
            List<Tag> tagsList = new List<Tag>();
            List<UserTaggedComment> userTaggedList = new List<UserTaggedComment>();
            string userName = item.User.UserName;
            tagsList = item.Tags.ToList();
            userTaggedList = item.UserTaggedComment.ToList();
            item.User = null;
            // Clear the post's tags and userTagged
            item.Tags.Clear();
            item.UserTaggedComment.Clear();
            // Add tags to comment -tag table
            tags = await AddTagsToComment(tagsList);
            // Get userId who posted the comment and insert to item
            int idUser = _userRepository.GetByUserName(userName).Id;
            item.UserId = idUser;
            // Add Comment to context
            var res = _context.Comments.Add(item);
            foreach (var tag in tags)
            {
                res.Entity.Tags.Add(tag);
            }
            // Add new userTagged to comment
            foreach (var userTagged in userTaggedList)
            {
                if (userTagged.User.UserName == "") break;
                int id = _userRepository.GetByUserName(userTagged.User.UserName).Id;
                res.Entity.UserTaggedComment.Add(new UserTaggedComment { UserId = id, CommentId = res.Entity.Id });
            }
            //res.Entity.User.UserName = userName;
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        private async Task<List<Tag>> AddTagsToComment(ICollection<Tag> tags)
        {
            return await _tagRepository.AddTags(tags);
        }

        public Task<Comment> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Edit(Comment item)
        {
            throw new NotImplementedException();
        }

        public ICollection<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Comment GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Comment> GetByPredicate(Func<Comment, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

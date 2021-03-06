using fakeLook_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Interfaces
{
    public interface IRepository<T>
    {
        public Task<T> Add(T item);
        public ICollection<T> GetAll();
        public Task<T> Edit(T item);
        public T GetById(int id);
        public ICollection<T> GetByPredicate(Func<T, bool> predicate);
        public Task<T> Delete(int id);
    }
    public interface IUserRepository : IRepository<User>
    {
        public User GetByUserName(string username);
        public Task<User> ChangePassword(string userName, string newPassword);
    }
    public interface IPostRepository : IRepository<Post>
    {
        public Task<Post> LikeUnLike(int postId, int userId);
        public string ConvetUserIdToUserName(int id);
        public Task<Post> AddCommentToPost(Comment comment);

    }
    public interface IGroupRepository : IRepository<Group>
    {
        public Task<Group> DeleteByGroupName(string groupName);
        public Group GetByGroupName(string groupName);
    }

    public interface ITagRepository : IRepository<Tag>
    {
        public Task<List<Tag>> AddTags(ICollection<Tag> tags);
    }

    public interface ICommentRepository : IRepository<Comment>
    {

    }
}

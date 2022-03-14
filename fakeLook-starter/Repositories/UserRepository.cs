using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly private DataContext _context;

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }


        public async Task<User> Add(User item)
        {
            var res = _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<User> Delete(int id)
        {
            var user = _context.Users.Where(user => user.Id == id).Single();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User> Edit(User item)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.SingleOrDefault(p => p.Id == id);
        }

        public ICollection<User> GetByPredicate(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate).ToList();
        }
    }
}

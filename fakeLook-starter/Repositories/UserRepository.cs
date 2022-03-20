using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fakeLook_starter.Services;


namespace fakeLook_starter.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly private DataContext _context;
        readonly private IDtoConverter _dtoConverter;

        public UserRepository(DataContext dataContext, IDtoConverter dtoConverter)
        {
            _context = dataContext;
            _dtoConverter = dtoConverter;
        }


        public async Task<User> Add(User item)
        {
            item.Password = Encryptions.MyEncryption(item.Password);
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
            return _context.Users.Select(u => _dtoConverter.DtoUser(u)).ToList();
        }

        public User GetById(int id)
        {
            return _dtoConverter.DtoUser(_context.Users.
                SingleOrDefault(u => u.Id == id));
        }

        public ICollection<User> GetByPredicate(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate).ToList();
        }

        public User GetByUserName(string username)
        {
            return _context.Users.SingleOrDefault(p => p.UserName == username);
        }
    }
}

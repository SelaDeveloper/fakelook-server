using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        readonly private DataContext _context;
        public GroupRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Group> Add(Group item)
        {
            var res = _context.Groups.Add(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Group> Delete(int id)
        {
            var group = _context.Groups.Where(group => group.Id == id).Single();
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> DeleteByGroupName(string groupName)
        {
            var group = _context.Groups.Where(group => group.GroupName == groupName).Single();
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public Task<Group> Edit(Group item)
        {
            throw new NotImplementedException();
        }

        public ICollection<Group> GetAll()
        {
            return _context.Groups.ToList();
        }

        public Group GetByGroupName(string groupName)
        {
            return _context.Groups.SingleOrDefault(g => g.GroupName == groupName);
        }

        public Group GetById(int id)
        {
            return _context.Groups.SingleOrDefault(g => g.Id == id);
        }

        public ICollection<Group> GetByPredicate(Func<Group, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

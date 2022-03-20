using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;

namespace fakeLook_starter.Repositories
{
    public class TagRepository : ITagRepository
    {
        readonly private DataContext _context;
        public TagRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public Task<Tag> Add(Tag item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tag>> AddTags(ICollection<Tag> tags)
        {
            List<Tag> addedTags = new List<Tag>();
            foreach (var tag in tags)
            {
                var t = _context.Tags.Where(t => t.Content == tag.Content).SingleOrDefault();
                if (t != null)
                {
                    addedTags.Add(t);
                }
                else
                {
                    var addedTag = _context.Tags.Add(tag);
                    addedTags.Add(addedTag.Entity);
                }
            }
            await _context.SaveChangesAsync();
            return addedTags;
        }


        public Task<Tag> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag> Edit(Tag item)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tag> GetAll()
        {
            throw new NotImplementedException();
        }

        public Tag GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Tag> GetByPredicate(Func<Tag, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

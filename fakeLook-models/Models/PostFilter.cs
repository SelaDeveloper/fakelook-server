using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    public class PostFilter
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public ICollection<string> Publishers { get; set; }
        public ICollection<string> Tags { get; set; }
        public ICollection<string> TaggedUsers { get; set; }


        public bool checkTaggedUsers(ICollection<UserTaggedPost> postTaggedUsers)
        {
            if (TaggedUsers == null || TaggedUsers.Count == 0) return true;
            if (TaggedUsers.Count == 1 && TaggedUsers.First().Equals("")) return true;
            if (postTaggedUsers == null || postTaggedUsers.Count == 0) return false;
            foreach (var filterTaggedUser in TaggedUsers)
            {
                foreach (var postTagUser in postTaggedUsers)
                {
                    if (filterTaggedUser.Equals(postTagUser.User.UserName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool checkTaggs(ICollection<Tag> postTags)
        {
            if (Tags == null || Tags.Count == 0) return true;
            if (Tags.Count == 1 && Tags.First().Equals("")) return true;
            if (postTags == null || postTags.Count == 0) return false;
            foreach (var tag in Tags)
            {
                foreach (var postTag in postTags)
                {
                    if (tag.Equals(postTag.Content))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool checkDate(DateTime postDate)
        {
            if (startDate.Equals(new DateTime(2000, 01, 01)) || endDate.Equals(new DateTime(2000, 01, 01))) return true;
            if (postDate >= startDate && postDate <= endDate)
                return true;
            return false;
        }

        public bool checkPublishers(string UserName)
        {
            if (Publishers == null || Publishers.Count == 0) return true;
            if (Publishers.Count == 1 && Publishers.First().Equals("")) return true;
            if (Publishers.Contains(UserName)) return true;
            return false;
        }

    }
}

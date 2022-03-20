using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    [Index(propertyNames: nameof(Content),
        IsUnique = true)]
    public class Tag
    {
        public int Id { get; set; }
        public string Content { get; set; }

        /* EF Relations */
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}

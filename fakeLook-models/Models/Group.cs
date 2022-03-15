using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_models.Models
{
    [Index(propertyNames: nameof(GroupName),
          IsUnique = true)]

    public class Group
    {
        public int Id { get; set; }

        [StringLength(450)]
        public string GroupName { get; set; }
        public virtual ICollection<User> Users { get; set; }


    }
}

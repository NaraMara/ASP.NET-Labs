using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models
{
    public class ForumCategory
    {
        public Guid Id { get; set; }=Guid.NewGuid();

        public String Name { get; set; }
        public ICollection<Forum> Forums { get; set; }
    }
}

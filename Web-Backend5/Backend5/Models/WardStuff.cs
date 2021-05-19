using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class WardStuff
    {
        public Int32 WardStuffId { get; set; }
        public Ward Ward { get; set; }
        public Int32 WardId { get; set; }
        [Required]
        [MaxLength(200)]
        public String Name{ get; set; }
        public String Position { get; set; }
    }
}

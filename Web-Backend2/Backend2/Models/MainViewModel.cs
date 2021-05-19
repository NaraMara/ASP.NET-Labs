using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend2.Models
{
    public class MainViewModel
    {
        [Required(ErrorMessage = "number is invalid")]
        public int First { get; set; }
        [Required(ErrorMessage = "number is invalid")]
        public int Second { get; set; }
        public String  Operator { get; set; }
        public double Result{ get; set; }
    }
}

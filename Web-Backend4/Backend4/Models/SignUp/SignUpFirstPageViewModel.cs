using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models.Controls;

namespace Backend4.Models.SignUp
{
    public  class SignUpFirstPageViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public  Month Month{ get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Gender { get; set; }


    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend4.Models.SignUp
{
    public class SignUpSecondPageViewModel:SignUpFirstPageViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword  { get; set; }
        [Required]
        public bool IsSelected { get; set; }
        [Required]
        public bool IsEverExisted { get; set; }
    }
}

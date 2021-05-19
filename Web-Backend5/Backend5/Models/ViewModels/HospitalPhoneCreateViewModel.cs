using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class HospitalPhoneCreateViewModel
    {
        [Required]
        [MaxLength(20)]
        public String Number { get; set; }
    }
}

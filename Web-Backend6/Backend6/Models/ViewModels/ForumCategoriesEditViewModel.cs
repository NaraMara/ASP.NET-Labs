﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend6.Models.ViewModels
{
    public class ForumCategoriesEditViewModel
    {
        [Required]
        public String Name { get; set; }
    }
}

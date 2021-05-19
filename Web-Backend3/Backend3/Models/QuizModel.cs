using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Models
{
    public class QuizModel
    {
        public int RightAnswCount { get; set; }
        public Int32 CurrentCount { get; set; }
        public Question NewQuestion { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}

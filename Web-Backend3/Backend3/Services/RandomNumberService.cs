using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Services
{
    public class RandomNumberService: IRandomNumberService
    {
        public Random Generator { get; set; } = new Random();
        
        public int GiveRandomNumberInRange(int lower,int upper)
        {
            return Generator.Next(lower, upper);
        }
        public string GiveRandomOperator()
        {
            var i = Generator.Next(0, 4);
            switch (i)
            {
                case 0: return "-";
                case 1: return "+";
                case 2: return "*";
                case 3: return "/";

                default:
                    return "smth went wrong ";
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
    public class RandomNumbersService : IRandomNumberService
    {
        public int first { get; set; }
        public int second { get; set; }

        public Random Generator { get; set; } 
        public RandomNumbersService()
        {
            Generator = new Random();
             
             
        }
        public int GiveRandomNumber(int lower, int upper)
        {
            return Generator.Next(lower,upper);
        }
        public void SetRandomNumbers(int lower, int upper)
        {
            first = Generator.Next();
            second = Generator.Next();
        }
        
        
    }
}

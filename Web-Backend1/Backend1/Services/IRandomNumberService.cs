using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
    public interface IRandomNumberService
    {
        int first { get; set; }
        int second { get; set; }

        int GiveRandomNumber(int lower,int upper);
         void SetRandomNumbers(int lower, int upper);
    }
}

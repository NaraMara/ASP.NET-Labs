using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Services
{
    public class CalculateService: ICalculateService
    {
        public double Calculate(int first, int second, string op)
        {
            switch (op)
            {
                case "-": return first - second;
                case "+": return first + second;
                case "*": return first * second;
                case "/": return first / second;
            }
            return 0;
        }
    }
}

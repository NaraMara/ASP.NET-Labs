using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
    public class OperatorsService:IOperatorsService
    {
        


        public int Multiply(int f, int s) 
        {
            return f * s;
        }
        public double Divide(int f, int s)
        {
            return f / s;
        }
        public int Sum(int f, int s)
        {
            return f + s;
        }

        public int Subtract(int f, int s)
        {
            return f - s;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend1.Services
{
   public interface IOperatorsService
    {
        
        int Multiply(int f, int s);
        double Divide(int f , int s );
        int Sum(int f, int s);
        int Subtract(int f, int s);

    }
}

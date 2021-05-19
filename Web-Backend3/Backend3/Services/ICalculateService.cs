using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Services
{
    public interface ICalculateService
    {
        double Calculate(int first, int second, string op);
    }
}

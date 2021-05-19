﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend3.Services
{
    public interface IRandomNumberService
    {
        int GiveRandomNumberInRange(int lower, int upper);
        string GiveRandomOperator();
    }
}

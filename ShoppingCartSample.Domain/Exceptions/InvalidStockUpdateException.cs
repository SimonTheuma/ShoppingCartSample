﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Exceptions
{
    public class InvalidStockUpdateException : Exception
    {
        public InvalidStockUpdateException(string message) : base(message)
        {
            
        }
    }
}

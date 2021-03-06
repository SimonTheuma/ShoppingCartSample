﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string UserID { get; set; }              
        public int CartID { get; set; }  
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => UnitPrice*(decimal)Quantity;
    }
}

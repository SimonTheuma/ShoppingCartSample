using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartSample.ViewModels
{
    public class UpdateOrderViewModel
    {
        public int OrderId { get; set; }
        public int NewQuantity { get; set; }
    }
}
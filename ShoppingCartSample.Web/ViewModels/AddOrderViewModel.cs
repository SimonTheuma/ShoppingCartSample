using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCartSample.ViewModels
{
    public class AddOrderViewModel
    {
        public int ProductID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}.")]
        public int Quantity { get; set; }
    }
}
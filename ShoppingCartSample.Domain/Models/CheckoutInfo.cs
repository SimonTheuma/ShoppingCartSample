using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models.Charges;

namespace ShoppingCartSample.Domain.Models
{
    public class CheckoutInfo
    {
        public ICollection<Order> Orders { get; set; }

        public ICollection<BaseCharge> ExtraCharges { get; set; }

        public ICollection<Discount> Discounts { get; set; }
    }
}
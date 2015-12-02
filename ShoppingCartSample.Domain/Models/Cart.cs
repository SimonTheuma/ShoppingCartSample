using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Models
{
    /// <summary>
    /// The shopping cart.
    /// </summary>
    public class Cart
    {        
        public int ID { get; set; }

        public string UserID { get; set; }

        public virtual ICollection<Order> Orders { get; set; }      

        /// <summary>
        /// Gets the total value of the cart.
        /// </summary>
        public decimal Total
        {
            get { return Orders.Sum(x => x.SubTotal); }
        }

        public DateTime LastUpdatedUtc { get; set; }

        public bool IsCheckedOut { get; set; }

        public bool IsCleared { get; set; }
    }
}

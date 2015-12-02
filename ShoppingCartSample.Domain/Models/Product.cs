using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Models
{
    public class Product
    {
        public int ID { get; set; }

        /// <summary>
        /// The price of the product. Ideally defined in the base currency.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The link to the product image.
        /// </summary>
        public string ImageUri { get; set; }

        /// <summary>
        /// The amount of items available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Tags to aid search.
        /// </summary>
        /// (and potentially SEO)
        public virtual ICollection<string> Tags { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Models
{
    public class Currency
    {
        public int ID { get; set; }

        /// <summary>
        /// 3-letter code (eg. EUR, GBP)
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 1-letter character symbol (eg. €)
        /// </summary>
        public string Symbol { get; set; }

        public bool IsDefault { get; set; }
    }
}

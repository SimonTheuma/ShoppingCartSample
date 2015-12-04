using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Domain.Models.Charges;

namespace ShoppingCartSample.Data.Contexts
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserActionLog> UserActionLogs { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

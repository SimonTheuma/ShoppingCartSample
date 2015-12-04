using System.Collections.Generic;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingCartSample.Data.Contexts.ShoppingCartContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShoppingCartSample.Data.Contexts.ShoppingCartContext context)
        {
            #region Products
            var products = new List<Product>
            {
                new Product() { Name = "Product 1", Description = "Super Cool Product 1", ImageUri = "http://placehold.it/250x250", Price = 2.00m, Quantity = 15 },
                new Product() { Name = "Product 2", Description = "Super Cool Product 2", ImageUri = "http://placehold.it/250x250", Price = 1.50m, Quantity = 25 },
                new Product() { Name = "Product 3", Description = "Super Cool Product 3", ImageUri = "http://placehold.it/250x250", Price = 3.00m, Quantity = 100 },
                new Product() { Name = "Product 4", Description = "Super Cool Product 4", ImageUri = "http://placehold.it/250x250", Price = 4.00m, Quantity = 4 },
                new Product() { Name = "Product 5", Description = "Super Cool Product 5", ImageUri = "http://placehold.it/250x250", Price = 10.00m, Quantity = 3 },
                new Product() { Name = "Product 6", Description = "Super Cool Product 6", ImageUri = "http://placehold.it/250x250", Price = 100.00m, Quantity = 2 }
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
            #endregion

            #region Currencies

            var currencies = new List<Currency>
            {
                new Currency(){Code = "USD", IsDefault = false, Symbol = "$"},
                new Currency(){Code = "GBP", IsDefault = false, Symbol = "£"},
                new Currency(){Code = "EUR", IsDefault = true, Symbol = "€"}
            };

            currencies.ForEach(c => context.Currencies.Add(c));
            context.SaveChanges();

            #endregion
        }
    }
}

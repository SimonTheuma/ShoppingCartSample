using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Contexts;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingCartContext _context = new ShoppingCartContext();

        public bool Exists(int productId)
        {
            return _context.Products.Any(p => p.ID == productId);
        }

        public bool IsStockAvailable(int productId, int quantity)
        {          
            var isAvailable = _context.Products.Any(product => product.ID == productId && product.Quantity >= quantity);
            return isAvailable;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

        public Product GetById(int productId)
        {            
            return _context.Products.FirstOrDefault(p => p.ID == productId);            
        }

        public void UpdateStockQuantity(Product product, int quantityPurchased)
        {            
            product.Quantity -= quantityPurchased;
            _context.SaveChanges();
        }

        public void UpdateStockQuantity(int productId, int quantityPurchased)
        {
            var product = _context.Products.First(p => p.ID == productId);
            UpdateStockQuantity(product, quantityPurchased);
        }
    }
}

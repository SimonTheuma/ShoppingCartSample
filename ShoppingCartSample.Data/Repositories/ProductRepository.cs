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

        public bool IsStockAvailable(int productId, int quantity)
        {
            ContinueIfProductExists(productId);

            if (quantity < 1)
            {
                throw new InvalidArgumentException("Invalid quantity amount specified.");
            }

            var isAvailable = _context.Products.Any(product => product.ID == productId && product.Quantity >= quantity);
            return isAvailable;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

        public Product GetById(int productId)
        {            
            var product = _context.Products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        public void UpdateStockQuantity(int productId, int quantityPurchased)
        {            
            var product = _context.Products.FirstOrDefault(p => p.ID == productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            if (product.Quantity - quantityPurchased < 0)
            {
                throw new InvalidStockUpdateException("Not enough quantity to update stock to new value.");
            }

            product.Quantity -= quantityPurchased;
            _context.SaveChanges();
        }

        public void ContinueIfProductExists(int productId)
        {
            if (productId == 0)
            {
                throw new InvalidArgumentException("ProductId was empty.");
            }

            if (!(_context.Products.Any(p => p.ID == productId)))
            {
                throw new ProductNotFoundException();
            }
        }
    }
}

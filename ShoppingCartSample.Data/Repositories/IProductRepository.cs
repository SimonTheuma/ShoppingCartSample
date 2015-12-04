using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Data.Repositories
{
    public interface IProductRepository
    {
        bool IsStockAvailable(int productId, int quantity);
        IEnumerable<Product> GetAll();
        Product GetById(int productId);
        void UpdateStockQuantity(Product product, int quantityPurchased);
        void UpdateStockQuantity(int productId, int quantityPurchased);
        bool Exists(int productId);
    }
}
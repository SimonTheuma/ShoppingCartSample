using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public interface IProductService
    {
        bool IsStockAvailable(int productId, int quantity);

        IEnumerable<Product> GetAll();

        Product GetById(int productId);

        void UpdateStockQuantity(int productId, int quantityPurchased);
    }
}

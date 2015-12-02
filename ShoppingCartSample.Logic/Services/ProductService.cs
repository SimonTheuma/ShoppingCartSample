using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public class ProductService : IProductService
    {
        public bool IsInStock(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            //TODO: convert price if current currency is not base
            throw new NotImplementedException();
        }

        public Product GetById(int productId)
        {
            //TODO: convert price if current currency is not base
            throw new NotImplementedException();
        }

        public void UpdateStockQuantity(int productId, int quantityPurchased)
        {
            throw new NotImplementedException();
        }

        public void UpdateStockQuantity(IEnumerable<Tuple<int,int>> productList)
        {
            foreach (var product in productList)
            {
                UpdateStockQuantity(product.Item1, product.Item2);
            }
        }
    }
}

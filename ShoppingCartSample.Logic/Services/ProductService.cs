using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Models;

namespace ShoppingCartSample.Logic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool IsInStock(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            //TODO: convert price if current currency is not base
            return _productRepository.GetAll();
        }

        public Product GetById(int productId)
        {
            //TODO: convert price if current currency is not base
            return _productRepository.GetById(productId);
        }

        public void UpdateStockQuantity(int productId, int quantityPurchased)
        {
            _productRepository.UpdateStockQuantity(productId, quantityPurchased);
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

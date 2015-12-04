using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Exceptions;
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

        public bool IsStockAvailable(int productId, int quantity)
        {
            ContinueIfProductExists(productId);

            if (quantity < 1)
            {
                throw new InvalidArgumentException("Invalid quantity amount specified.");
            }

            return _productRepository.IsStockAvailable(productId, quantity);
        }

        public IEnumerable<Product> GetAll()
        {
            //TODO: convert price if current currency is not base
            return _productRepository.GetAll();
        }

        public Product GetById(int productId)
        {
            //TODO: convert price if current currency is not base            
            if (!_productRepository.Exists(productId))
            {
                throw new ProductNotFoundException();
            }

            return _productRepository.GetById(productId);
        }

        public void UpdateStockQuantity(int productId, int quantityPurchased)
        {
            var product = GetById(productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            if (product.Quantity - quantityPurchased < 0)
            {
                throw new InvalidStockUpdateException("Not enough quantity to update stock to new value.");
            }

            _productRepository.UpdateStockQuantity(product, quantityPurchased);
        }

        public void UpdateStockQuantity(IEnumerable<Tuple<int,int>> productList)
        {
            foreach (var product in productList)
            {
                UpdateStockQuantity(product.Item1, product.Item2);
            }
        }

        private void ContinueIfProductExists(int productId)
        {
            if (productId == 0)
            {
                throw new InvalidArgumentException("ProductId was empty.");
            }

            if (!(_productRepository.Exists(productId)))
            {
                throw new ProductNotFoundException();
            }
        }
    }
}

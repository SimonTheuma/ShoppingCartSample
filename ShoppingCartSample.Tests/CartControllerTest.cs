using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCartSample.Controllers;
using ShoppingCartSample.Domain.Exceptions;

namespace ShoppingCartSample.Tests
{
    [TestClass]
    public class CartControllerTest
    {        
        [TestMethod]
        public void TestGetProducts()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void TestAddNullProductToCart()
        {            
            
        }

        [TestMethod]
        [ExpectedException(typeof(ProductOutOfStockException))]
        public void TestAddOutOfStockProductToCart()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStockUpdateException))]
        public void TestAddNegativeQuantityOfProductToCart()
        {                        
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestGetCartForUnknownUser()
        {            
        }

        [TestMethod]        
        public void TestGetNullCartForUser()
        {
            //should get a new cart
        }

        [TestMethod]
        [ExpectedException(typeof(ProductOutOfStockException))]
        public void TestCheckoutProductMoreQuantityThanInStock()
        {            
        }

        [TestMethod]
        [ExpectedException(typeof(CartNotFoundException))]
        public void TestCheckoutInactiveCart()
        {            
        }

        [TestMethod]
        [ExpectedException(typeof(OrderNotFoundException))]
        public void TestRemoveNullOrder()
        {            
        }

        [TestMethod]
        [ExpectedException(typeof(OrderNotFoundException))]
        public void TestUpdateNullOrder()
        {            
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestTransferCartFromUnknownUser()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestTransferCartToUnknownUser()
        {            
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyHasCartException))]
        public void TestTransferCartToUserWhoAlreadyHasCartWithoutOverwrite()
        {
        }

        [TestMethod]        
        public void TestTransferCartToUserWhoAlreadyHasCartWithOverwrite()
        {
        }
    }
}

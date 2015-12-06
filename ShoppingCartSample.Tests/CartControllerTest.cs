using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ShoppingCartSample.Authentication.Services;
using ShoppingCartSample.Controllers;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Logic.Services;
using ShoppingCartSample.ViewModels;

namespace ShoppingCartSample.Tests
{
    [TestClass]
    public class CartControllerTest
    {
        private Mock<ICartService> _cartServiceMock;
        private Mock<IUserService> _userServiceMock;
        CartController _cartController;
        List<Cart> carts;        

        private const string VALID_ID = "c8b8de25-06f7-4904-ba1e-4b646167c966";
        private const string INVALID_ID = "blabla";

        [TestInitialize]
        public void Initialize()
        {
            _cartServiceMock = new Mock<ICartService>();
            _userServiceMock = new Mock<IUserService>();

            var mockHttpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();

            _cartController = new CartController(_cartServiceMock.Object, _userServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };            

            mockHttpContext.SetupGet(x => x.Response).Returns(response.Object);
        }

        /*
        [TestMethod]
        public void TestAddValidProductToCart()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);      

            _cartServiceMock.Setup(x => x.AddOrder(VALID_ID, 1, 1))
                .Returns(new Order()
                {
                    ID = 1,
                    CartID = 1,
                    ProductID = 1,
                    ProductName = "test",
                    Quantity = 1,
                    UnitPrice = 1,
                    UserID = VALID_ID
                });

            //Act
            var result = (JsonResult) _cartController.AddOrder(
                new AddOrderViewModel()
                {
                    ProductID = 1,
                    Quantity = 1
                });

            dynamic order = result.Data as dynamic;

            //Assert
            Assert.AreEqual("test", order.productName.ToString());            
        }
        */

        [TestMethod]        
        public void TestAddInvalidProductToCart()
        {
            //Arrange            
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.AddOrder(VALID_ID, 0, 1)).Throws(new ProductNotFoundException());            

            //Act
            var result = ((HttpStatusCodeResult) _cartController.AddOrder(
                new AddOrderViewModel()
                {
                    ProductID = 0,
                    Quantity = 1
                }));

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("product", result.StatusDescription);
        }

        [TestMethod]        
        public void TestAddOutOfStockProductToCart()
        {
            //Arrange            
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);            
            _cartServiceMock.Setup(x => x.AddOrder(VALID_ID, 1, 1)).Throws(new ProductOutOfStockException());

            //Act
            var result = ((HttpStatusCodeResult)_cartController.AddOrder(
                new AddOrderViewModel()
                {
                    ProductID = 1,
                    Quantity = 1
                }));

            //Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("stock", result.StatusDescription);
        }

        [TestMethod] 
        //TODO: Finish       
        public void TestAddNegativeQuantityOfProductToCart()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);


            //Act
            var result = ((JsonResult)_cartController.AddOrder(
                new AddOrderViewModel()
                {
                    ProductID = 1,
                    Quantity = -1
                }));               

            //Assert
            //Assert.AreEqual(result.);
            //Assert.AreEqual(errorMessage, "Please enter a value bigger than 1.");
            _cartServiceMock.Verify(x => x.AddOrder(VALID_ID, 1, -1), Times.Never);
        }

        [TestMethod]        
        public void TestGetCartForUnknownUser()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(INVALID_ID);
            _cartServiceMock.Setup(x => x.GetByUserId(INVALID_ID)).Throws(new UserNotFoundException());

            //Act
            var result = (HttpStatusCodeResult) _cartController.GetCart();

            Assert.AreEqual("404", (int)result.StatusCode);
            Assert.AreEqual("user", result.StatusDescription);
        }

        [TestMethod]
        public void TestGetCartForUser()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.GetByUserId(VALID_ID)).Returns(new Cart() { UserID = VALID_ID, Orders = new List<Order>(), LastUpdatedUtc = DateTime.UtcNow });           

            //Act
            var result = (JsonResult)_cartController.GetCart();
            Cart cart = result.Data as Cart;

            //Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(VALID_ID, cart.UserID);
            Assert.AreEqual(0, cart.Orders.Count);
            Assert.IsFalse(cart.IsCheckedOut);
            Assert.IsFalse(cart.IsCleared);
        }

        [TestMethod]        
        public void TestGetNewCartForUser()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.GetByUserId(VALID_ID)).Throws(new CartNotFoundException());
            _cartServiceMock.Setup(x => x.Create(VALID_ID)).Returns(new Cart() { UserID = VALID_ID, Orders = new List<Order>(), LastUpdatedUtc = DateTime.UtcNow });

            //Act
            var result = (JsonResult) _cartController.GetCart();
            Cart cart = result.Data as Cart;

            //Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(VALID_ID, cart.UserID);
            Assert.AreEqual(0, cart.Orders.Count);
            Assert.IsFalse(cart.IsCheckedOut);
            Assert.IsFalse(cart.IsCleared);            
        }

        [TestMethod]
        public void TestValidCheckoutCreatesNewCart()
        {
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.Create(VALID_ID)).Returns(new Cart() { UserID = VALID_ID, Orders = new List<Order>(), LastUpdatedUtc = DateTime.UtcNow });

            var result = (JsonResult) _cartController.ProcessCheckout();
            Cart cart = result.Data as Cart;

            //Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(VALID_ID, cart.UserID);
            Assert.AreEqual(0, cart.Orders.Count);
            Assert.IsFalse(cart.IsCheckedOut);
            Assert.IsFalse(cart.IsCleared);

            _cartServiceMock.Verify(x => x.ProcessCheckout(VALID_ID), Times.Once);
            _cartServiceMock.Verify(x => x.Create(VALID_ID), Times.Once);
        }

        [TestMethod]        
        public void TestCheckoutProductMoreQuantityThanInStock()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.ProcessCheckout(VALID_ID)).Throws(new ProductOutOfStockException());

            //Act
            var result = (HttpStatusCodeResult) _cartController.ProcessCheckout();

            //Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("stock", result.StatusDescription);

        }        

        [TestMethod]        
        public void TestRemoveNullOrder()
        {
            //Arrange
            _userServiceMock.Setup(x => x.GetUserId()).Returns(VALID_ID);
            _cartServiceMock.Setup(x => x.RemoveOrder(VALID_ID, 0)).Throws(new OrderNotFoundException());

            //Act
            var result = (HttpStatusCodeResult) _cartController.RemoveOrder(
                new RemoveOrderViewModel()
                {
                    OrderId = 0
                });

            //Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("order", result.StatusDescription);
        }       
    }
}

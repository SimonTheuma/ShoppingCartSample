using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Authentication.Services;
using ShoppingCartSample.Data.Contexts;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Exceptions;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Domain.Models.Charges;
using ShoppingCartSample.Domain.Models.UserActions;

namespace ShoppingCartSample.Logic.Services
{
    public class CartService : ICartService
    {        
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IAuditService auditService, ICartRepository cartRepository, IUserService userService, IProductRepository productRepository)
        {
            _auditService = auditService;
            _cartRepository = cartRepository;
            _userService = userService;
            _productRepository = productRepository;
        }

        public void ProcessCheckout(string userId)
        {
            ContinueIfUserExists(userId);

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }

            var somethingNotInStock = _cartRepository.SomethingNotInStock(cart);

            if (somethingNotInStock)
            {
                throw new ProductOutOfStockException();
            }

            _cartRepository.CheckOutCart(cart);
            _auditService.LogUserAction(new UserCheckedOut(userId));
        }        

        public Cart Create(string userId)
        {
            ContinueIfUserExists(userId);

            if (_cartRepository.UserHasCart(userId))
            {
                throw new UserAlreadyHasCartException();
            }

            var cart = new Cart
            {
                UserID = userId,
                LastUpdatedUtc = DateTime.UtcNow,
                IsCheckedOut = false,
                IsCleared = false,
                Orders = new List<Order>()
            };

            _cartRepository.Create(cart);

            return cart;
        }

        public Cart GetByUserId(string userId)
        {
            ContinueIfUserExists(userId);

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }

            return cart;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">The User's Id</param>
        /// <param name="order">The Order object.</param>
        /// <returns>The Order Id of the newly created order.</returns>
        public Order AddOrder(string userId, int productId, int quantity)
        {
            ContinueIfUserExists(userId);

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }

            var product = _productRepository.GetById(productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            Order newOrder = _cartRepository.AddOrder(cart, product, quantity);
            _auditService.LogUserAction(new UserAddedItemToCart(userId, productId, quantity));

            return newOrder;
        }

        public void RemoveOrder(string userId, int orderId)
        {
            ContinueIfUserExists(userId);

            var order = _cartRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new OrderNotFoundException();
            }

            var productId = order.ProductID;
            var quantity = order.Quantity;

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }                        

            if (order.UserID != userId)
            {
                throw new OrderDoesNotBelongToUserException();
            }

            _cartRepository.RemoveOrder(cart, order);

            _auditService.LogUserAction(new UserRemovedItemFromCart(userId, productId, quantity));
        }

        public void UpdateOrder(string userId, int orderId, int newQuantity)
        {
            ContinueIfUserExists(userId);

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }

            var order = _cartRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new OrderNotFoundException();
            }

            _cartRepository.UpdateOrder(cart, order, newQuantity);
        }

        public void Clear(string userId)
        {
            ContinueIfUserExists(userId);

            var cart = _cartRepository.GetByUserId(userId);

            if (cart == null)
            {
                throw new CartNotFoundException();
            }

            _cartRepository.Clear(cart);
            _auditService.LogUserAction(new UserClearedCart(userId));
        }

        public void Transfer(string sourceUserId, string targetUserId, bool overwriteCart = false)
        {
            //Check that both users exist.
            ContinueIfUserExists(sourceUserId);

            //Only temporary users can transfer their cart.
            if (!_userService.IsTemporary(sourceUserId) && !overwriteCart)
            {
                throw new InvalidArgumentException("Non-temporary user attempted to transfer cart.");
            }

            ContinueIfUserExists(targetUserId);

            var sourceCart = _cartRepository.GetByUserId(sourceUserId);

            if (sourceCart == null)
            {
                throw new CartNotFoundException();
            }

            var targetCart = _cartRepository.GetByUserId(targetUserId);

            if (targetCart != null)
            {
                var userCartIsEmpty = _cartRepository.UserHasEmptyCart(targetUserId);

                if (!userCartIsEmpty && !overwriteCart)
                {
                    throw new UserAlreadyHasCartException();
                }

                _cartRepository.Clear(targetCart);
            }            

            _cartRepository.Transfer(sourceCart, targetUserId);
        }

        public bool HasEmptyCart(string userId)
        {
            return _cartRepository.UserHasEmptyCart(userId);
        }

        private void ContinueIfUserExists(string userId)
        {
            if (!_userService.Exists(userId))
            {
                throw new UserNotFoundException();
            }
        }
    }
}

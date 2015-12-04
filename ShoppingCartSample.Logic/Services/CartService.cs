using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Data.Contexts;
using ShoppingCartSample.Data.Repositories;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Domain.Models.Charges;
using ShoppingCartSample.Domain.Models.UserActions;

namespace ShoppingCartSample.Logic.Services
{
    public class CartService : ICartService
    {        
        private readonly IAuditService _auditService;
        private readonly ICartRepository _cartRepository;

        public CartService(IAuditService auditService, ICartRepository cartRepository)
        {
            _auditService = auditService;
            _cartRepository = cartRepository;
        }

        public Cart Create(string userId)
        {
            var cart = _cartRepository.Create(userId);
            return cart;
        }

        public void ProcessCheckout(string userId)
        {
            _cartRepository.ProcessCheckout(userId);
            _auditService.LogUserAction(new UserCheckedOut(userId));
        }        

        public IEnumerable<Discount> GetDiscounts(string userId, params string[] discounts)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseCharge> GetExtraCharges(string userId, params string[] charges)
        {
            throw new NotImplementedException();
        }

        public void CreateCart(string userId)
        {
            _cartRepository.Create(userId);
        }

        public Cart GetByUserId(string userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">The User's Id</param>
        /// <param name="order">The Order object.</param>
        /// <returns>The Order Id of the newly created order.</returns>
        public Order AddOrder(string userId, int productId, int quantity)
        {
            Order newOrder = _cartRepository.AddOrder(userId, productId, quantity);
            _auditService.LogUserAction(new UserAddedItemToCart(userId, productId, quantity));

            return newOrder;
        }

        public void RemoveOrder(string userId, int orderId)
        {
            var order = _cartRepository.GetOrderById(orderId);

            var productId = order.ProductID;
            var quantity = order.Quantity;

            _cartRepository.RemoveOrder(userId, orderId);
            _auditService.LogUserAction(new UserRemovedItemFromCart(userId, productId, quantity));
        }

        public void UpdateOrder(string userId, int orderId, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public void Clear(string userId)
        {
            _cartRepository.Clear(userId);
            _auditService.LogUserAction(new UserClearedCart(userId));
        }

        public void Transfer(string sourceUserId, string targetUserId, bool overwriteCart = false)
        {
            _cartRepository.Transfer(sourceUserId, targetUserId, overwriteCart);
        }
    }
}

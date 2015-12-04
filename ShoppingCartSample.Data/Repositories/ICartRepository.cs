using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Models;
using ShoppingCartSample.Domain.Models.Charges;

namespace ShoppingCartSample.Data.Repositories
{
    public interface ICartRepository
    {
        void ProcessCheckout(string userId);

        Cart Create(string userId);

        Cart GetByUserId(string userId);

        Order AddOrder(string userId, int productId, int quantity);
        void RemoveOrder(string userId, int orderId);
        void UpdateOrder(string userId, int orderId, int newQuantity);
        Order GetOrderById(int orderId);
        void Clear(string userId);
        void Transfer(string sourceUserId, string targetUserId, bool overwriteCart = false);

        bool HasEmptyCart(string userId);
    }
}

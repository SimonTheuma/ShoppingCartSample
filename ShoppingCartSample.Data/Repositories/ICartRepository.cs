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
        void Create(Cart cart);
        Cart GetByUserId(string userId);
        Order AddOrder(Cart cart, Product product, int quantity);
        void RemoveOrder(Cart cart, Order order);
        void UpdateOrder(Cart cart, Order order, int newQuantity);
        Order GetOrderById(int orderId);
        void Clear(Cart cart);
        void Transfer(Cart sourceCart, string targetUserId);        
        bool SomethingNotInStock(Cart cart);
        void CheckOutCart(Cart cart);
        bool UserHasEmptyCart(string userId);
        bool UserHasCart(string userId, bool canBeEmpty = false);
    }
}
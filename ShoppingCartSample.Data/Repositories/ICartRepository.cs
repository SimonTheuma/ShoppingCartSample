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

        void CreateCart(string userId);

        IEnumerable<Discount> GetDiscounts(string userId, params string[] discounts);

        IEnumerable<BaseCharge> GetExtraCharges(string userId, params string[] charges);

        Cart GetByUserId(string userId);

        int AddOrder(Order order);
        void RemoveOrder(string userId, int orderId);
        void UpdateOrder(string userId, int orderId, int newQuantity);
        Order GetOrderById(int orderId);
        void Clear(string userId);
        void Transfer(string sourceUserId, string targetUserId, bool overwriteCart = false);
    }
}

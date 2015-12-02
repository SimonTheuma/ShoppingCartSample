using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.UserActions
{
    public class UserRemovedItemFromCart : UserActionBase
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public UserRemovedItemFromCart(string _userId, int _productID, int _quantity) : base(_userId)
        {            
            ProductID = _productID;
            Quantity = _quantity;
        }

        public override UserActionType GetUserActionType()
        {
            return UserActionType.RemovedItemFromCart;
        }

        public override string ToString()
        {
            return $"User {UserID} removed {Quantity} of {ProductID} from cart.";
        }
    }
}

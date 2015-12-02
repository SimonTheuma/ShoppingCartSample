using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.UserActions
{
    public class UserAddedItemToCart : UserActionBase
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public UserAddedItemToCart(string _userID, int _productID, int _quantity) : base(_userID)
        {            
            ProductID = _productID;
            Quantity = _quantity;
        }

        public override UserActionType GetUserActionType()
        {
            return UserActionType.AddedItemToCart;
        }

        public override string ToString()
        {
            return $"User {UserID} added {Quantity} of {ProductID} to cart.";
        }
    }
}

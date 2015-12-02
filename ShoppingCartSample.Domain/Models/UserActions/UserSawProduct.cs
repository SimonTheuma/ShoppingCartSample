using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.UserActions
{
    public class UserSawProduct : UserActionBase
    {
        public int ProductID { get; set; }

        public UserSawProduct(string _userId, int _productID) : base(_userId)
        {            
            ProductID = _productID;         
        }

        public override UserActionType GetUserActionType()
        {
            return UserActionType.SawProduct;
        }

        public override string ToString()
        {
            return $"User {UserID} saw {ProductID}.";
        }
    }
}

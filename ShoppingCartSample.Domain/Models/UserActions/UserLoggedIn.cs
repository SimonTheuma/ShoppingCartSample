using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.UserActions
{
    public class UserLoggedIn : UserActionBase
    {     
        public UserLoggedIn(string _userId) : base(_userId)
        {            
        }

        public override UserActionType GetUserActionType()
        {
            return UserActionType.Login;
        }

        public override string ToString()
        {
            return $"User {UserID} has logged in.";
        }
    }
}

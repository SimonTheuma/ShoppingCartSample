using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.UserActions
{
    public class UserLoggedOut : UserActionBase
    {
        public UserLoggedOut(string _userId) : base(_userId)
        {            
        }

        public override UserActionType GetUserActionType()
        {
            return UserActionType.Logout;
        }

        public override string ToString()
        {
            return $"User {UserID} has logged out.";
        }
    }
}

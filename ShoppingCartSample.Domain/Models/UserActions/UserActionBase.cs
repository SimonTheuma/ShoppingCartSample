using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models
{
    public abstract class UserActionBase
    {
        public string UserID { get; set; }

        protected UserActionBase(string _userId)
        {
            UserID = _userId;
        }

        public abstract UserActionType GetUserActionType();
    }
}

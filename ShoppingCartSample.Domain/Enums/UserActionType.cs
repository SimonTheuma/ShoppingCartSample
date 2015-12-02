using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartSample.Domain.Enums
{
    public enum UserActionType
    {
        Login, Logout, SawProduct, AddedItemToCart, RemovedItemFromCart, ClearedCart, CheckedOut
    }
}

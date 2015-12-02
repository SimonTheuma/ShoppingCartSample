using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models
{
    public class Audit
    {
        public int ID { get; set; }
        
        public string UserID { get; set; }

        public int UserActionId { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }        

        public Audit(UserActionBase baseAction)
        {
            UserID = baseAction.UserID;
            UserActionId = (int)baseAction.GetUserActionType();
            Message = baseAction.ToString();
            Timestamp = DateTime.UtcNow;
        }

    }
}

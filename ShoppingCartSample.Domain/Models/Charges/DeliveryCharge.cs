using ShoppingCartSample.Domain.Enums;

namespace ShoppingCartSample.Domain.Models.Charges
{
    public class DeliveryCharge : BaseCharge
    {
        public DeliveryChargeType Type { get; set; }
    }
}

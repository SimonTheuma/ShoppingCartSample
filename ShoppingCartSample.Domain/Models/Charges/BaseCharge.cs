namespace ShoppingCartSample.Domain.Models.Charges
{
    /// <summary>
    /// Represents an additional charge over and above the grand total. A negative charge can also be used as a discount.
    /// </summary>
    public class BaseCharge
    {
        public int ID { get; set; }

        public string Description { get; set; }
        public virtual decimal FlatAmount { get; set; }
        public virtual decimal PercentageAmount { get; set; }
    }
}
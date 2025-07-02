namespace Mercatika.Domain
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime DatePayment { get; set; }
        public int? CreditCardNum { get; set; }
        public string Estado { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
    }
}

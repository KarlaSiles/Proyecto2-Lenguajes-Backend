namespace Mercatika.Domain
{
    public class PaymentUpdateDto
    {
        public int PaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public int? CreditCardNum { get; set; }
    }
}

using System.Runtime.Intrinsics.X86;

namespace Mercatika.Domain
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public int Amount { get; set; }
        public decimal LinePrice { get; set; }

    }

}
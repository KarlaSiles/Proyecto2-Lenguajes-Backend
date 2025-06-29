namespace Mercatika.Domain
{

    public class Product
    {
        private int productId;
        private string productName;
        private decimal price;
        private Category categoryCode;
        private List<ProductDetail> productDetails;

        public Product()
        {
            productDetails = new List<ProductDetail>();
        }

        public Product(int productId, string productName, decimal price, Category categoryCode)
        {
            this.productId = productId;
            this.productName = productName;
            this.price = price;
            this.categoryCode = categoryCode;
            this.productDetails = new List<ProductDetail>();
        }

        public int ProductId
        {
            get => productId;
            set => productId = value;
        }

        public string ProductName
        {
            get => productName;
            set => productName = value;
        }

        public decimal Price
        {
            get => price;
            set => price = value;
        }

        public Category CategoryCode
        {
            get => categoryCode;
            set => categoryCode = value;
        }

        public List<ProductDetail> ProductDetails
        {
            get => productDetails;
            set => productDetails = value;
        }
    }

}


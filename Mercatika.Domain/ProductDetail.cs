using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class ProductDetail
    {
        private int productDetailtId;
        private int stockAmount;
        private String uniqueProductCode;
        private string size;
        private Product product;
        public ProductDetail()
        {
        }

        public ProductDetail(int productDetailtId, int stockAmount, string uniqueProductCode, string size, Product product)
        {
            this.productDetailtId = productDetailtId;
            this.stockAmount = stockAmount;
            this.uniqueProductCode = uniqueProductCode;
            this.size = size;
            this.product = product;
        }

        public int ProductDetailtId
        {
            get => productDetailtId;
            set => productDetailtId = value;
        }

        public int StockAmount
        {
            get => stockAmount;
            set => stockAmount = value;
        }

        public string UniqueProductCode
        {
            get => uniqueProductCode;
            set => uniqueProductCode = value;
        }
        public string Size
        {
            get => size;
            set => size = value;
        }
        public Product Product
        {
            get => product;
            set => product = value;
        }
    }
}

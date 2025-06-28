using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }

        public int StockAmount { get; set; }

        public string? UniqueProductCode { get; set; }  

        public string? Size { get; set; }               

        public Product? Product { get; set; }          
    }

}


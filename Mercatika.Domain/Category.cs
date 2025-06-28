using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class Category
    {
        private int categoryCode;
        private string description;


        public Category()
        {
        }

        public Category(int categoryCode, string description)
        {
            this.categoryCode = categoryCode;
            this.description = description;

        }

        public int CategoryCode
        {
            get => categoryCode;
            set => categoryCode = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }
    }
}

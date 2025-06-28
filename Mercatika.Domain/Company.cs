using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class Company
    {
        private int idsetup;
        private double sale_tax;
        private string name_company;
        private string address_company;
        private string city_company;
        private string state_or_province;
        private int zip_code_company;
        private string country_company;
        private int phone_company;
        private int fax_number_company;
        private string payments_terms;
        private string message;

        public Company() { }

        public Company(int idsetup, double sale_tax, string name_company, string address_company,
                      string city_company, string state_or_province, int zip_code_company,
                      string country_company, int phone_company, int fax_number_company,
                      string payments_terms, string message)
        {
            this.idsetup = idsetup;
            this.sale_tax = sale_tax;
            this.name_company = name_company;
            this.address_company = address_company;
            this.city_company = city_company;
            this.state_or_province = state_or_province;
            this.zip_code_company = zip_code_company;
            this.country_company = country_company;
            this.phone_company = phone_company;
            this.fax_number_company = fax_number_company;
            this.payments_terms = payments_terms;
            this.message = message;
        }

        public int Idsetup
        {
            get => idsetup;
            set => idsetup = value;
        }

        public double Sale_tax
        {
            get => sale_tax;
            set => sale_tax = value;
        }

        public string Name_company
        {
            get => name_company;
            set => name_company = value;
        }

        public string Address_company
        {
            get => address_company;
            set => address_company = value;
        }

        public string City_company
        {
            get => city_company;
            set => city_company = value;
        }

        public string State_or_province
        {
            get => state_or_province;
            set => state_or_province = value;
        }

        public int Zip_code_company
        {
            get => zip_code_company;
            set => zip_code_company = value;
        }

        public string Country_company
        {
            get => country_company;
            set => country_company = value;
        }

        public int Phone_company
        {
            get => phone_company;
            set => phone_company = value;
        }

        public int Fax_number_company
        {
            get => fax_number_company;
            set => fax_number_company = value;
        }

        public string Payments_terms
        {
            get => payments_terms;
            set => payments_terms = value;
        }

        public string Message
        {
            get => message;
            set => message = value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class Invoice
    {
        // — Datos de la orden
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Estado { get; set; }

        // — Datos de cliente (completo según tu clase Client)
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string ContractName { get; set; }
        public string ContractLastname { get; set; }
        public string ContractPosition { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int ZipCode { get; set; }
        public string Country { get; set; }
        public int Phone { get; set; }
        public int FaxNumber { get; set; }

        // — Líneas de detalle
        public List<InvoiceLine> Lines { get; set; } = new();

        // — Totales
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        // — Parámetros de la compañía emisora
        public double SaleTaxRate { get; set; }
        public string NameCompany { get; set; }
        public string AddressCompany { get; set; }
        public string CityCompany { get; set; }
        public string StateOrProvince { get; set; }
        public int ZipCodeCompany { get; set; }
        public string CountryCompany { get; set; }
        public int PhoneCompany { get; set; }
        public int FaxNumberCompany { get; set; }
        public string PaymentsTerms { get; set; }
        public string Message { get; set; }
    }

    public class InvoiceLine
    {
        public int ProductDetailId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineSubtotal { get; set; }
        public decimal LineTax { get; set; }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.Domain;

namespace Mercatika.DataAccess
{
    public class ReportData
    {
        private readonly string _conn;
        public ReportData(string conn) => _conn = conn;

        // 1) Listado general
        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
            => await GetInvoicesInternalAsync(null);

        // 2) Filtrado
        public async Task<IEnumerable<Invoice>> GetFilteredInvoicesAsync(
            int? clientId,
            int? orderId,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? estado)
        {
            var filters = new
            {
                clientId,
                orderId,
                dateFrom,
                dateTo,
                estado
            };
            return await GetInvoicesInternalAsync(filters);
        }

        // 3) Factura individual con detalle completo
        public async Task<Invoice> GetInvoiceByOrderIdAsync(int orderId)
        {
            var invoice = new Invoice();
            using var conn = new SqlConnection(_conn);
            await conn.OpenAsync();

            // — Traer orden + cliente
            const string sqlOrder = @"
                SELECT 
                  o.order_id, o.order_date, o.estado,
                  c.client_id, c.company_name, c.contract_name, c.contract_lastname, c.contract_position,
                  c.address, c.city, c.province, c.zip_code, c.country, c.phone, c.fax_number
                FROM [Order] o
                JOIN Client c ON o.client_id = c.client_id
                WHERE o.order_id = @orderId";
            using (var cmd = new SqlCommand(sqlOrder, conn))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                using var rdr = await cmd.ExecuteReaderAsync();
                if (!await rdr.ReadAsync())
                    throw new KeyNotFoundException($"Order {orderId} no encontrada");

                invoice.OrderId = rdr.GetInt32(0);
                invoice.OrderDate = rdr.GetDateTime(1);
                invoice.Estado = rdr.GetString(2);

                invoice.ClientId = rdr.GetInt32(3);
                invoice.CompanyName = rdr.GetString(4);
                invoice.ContractName = rdr.GetString(5);
                invoice.ContractLastname = rdr.GetString(6);
                invoice.ContractPosition = rdr.GetString(7);
                invoice.Address = rdr.GetString(8);
                invoice.City = rdr.GetString(9);
                invoice.Province = rdr.GetString(10);
                invoice.ZipCode = rdr.GetInt32(11);
                invoice.Country = rdr.GetString(12);
                invoice.Phone = rdr.GetInt32(13);
                invoice.FaxNumber = rdr.GetInt32(14);
            }

            // — Líneas de detalle
            const string sqlLines = @"
                SELECT 
                  od.productDetail_id, p.product_name, pd.stock_amount, od.amount, od.line_price
                FROM OrderDetail od
                JOIN ProductDetail pd ON od.productDetail_id = pd.productDetail_id
                JOIN Product p ON pd.product_id = p.product_id
                WHERE od.order_id = @orderId";
            decimal sub = 0m;
            using (var cmd = new SqlCommand(sqlLines, conn))
            {
                cmd.Parameters.AddWithValue("@orderId", orderId);
                using var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    var line = new InvoiceLine
                    {
                        ProductDetailId = rdr.GetInt32(0),
                        ProductName = rdr.GetString(1),
                        UnitPrice = rdr.GetDecimal(4) / rdr.GetInt32(3), // si line_price = precio * cantidad
                        Quantity = rdr.GetInt32(3),
                        LineSubtotal = rdr.GetDecimal(4)
                    };
                    invoice.Lines.Add(line);
                    sub += line.LineSubtotal;
                }
            }

            // — Parámetros de la compañía
            const string sqlComp = @"
                SELECT 
                  idsetup, sale_tax, name_company, address_company,
                  city_company, state_or_province, zip_code_company, country_company,
                  phone_company, fax_number_company, payments_terms, message
                FROM Company
                WHERE idsetup = 1";
            double taxRate = 0;
            using (var cmd = new SqlCommand(sqlComp, conn))
            {
                using var rdr = await cmd.ExecuteReaderAsync();
                if (await rdr.ReadAsync())
                {
                    invoice.SaleTaxRate = rdr.GetDouble(1);
                    invoice.NameCompany = rdr.GetString(2);
                    invoice.AddressCompany = rdr.GetString(3);
                    invoice.CityCompany = rdr.GetString(4);
                    invoice.StateOrProvince = rdr.GetString(5);
                    invoice.ZipCodeCompany = rdr.GetInt32(6);
                    invoice.CountryCompany = rdr.GetString(7);
                    invoice.PhoneCompany = rdr.GetInt32(8);
                    invoice.FaxNumberCompany = rdr.GetInt32(9);
                    invoice.PaymentsTerms = rdr.GetString(10);
                    invoice.Message = rdr.GetString(11);
                    taxRate = rdr.GetDouble(1);
                }
            }

            // — Calcular totales
            decimal taxTotal = 0m;
            foreach (var ln in invoice.Lines)
            {
                ln.LineTax = Math.Round(ln.LineSubtotal * (decimal)taxRate, 2);
                taxTotal += ln.LineTax;
            }

            invoice.Subtotal = sub;
            invoice.TaxAmount = taxTotal;
            invoice.Total = sub + taxTotal;

            return invoice;
        }

        // — interno para listado y filtrado (sin detalle ni totales)
        private async Task<IEnumerable<Invoice>> GetInvoicesInternalAsync(object? filters)
        {
            var list = new List<Invoice>();
            using var conn = new SqlConnection(_conn);
            await conn.OpenAsync();

            var sb = new StringBuilder(@"
                SELECT 
                  o.order_id, o.order_date, o.estado,
                  c.client_id, c.company_name
                FROM [Order] o
                JOIN Client c ON o.client_id = c.client_id
                WHERE 1=1
            ");
            using var cmd = new SqlCommand { Connection = conn };

            if (filters != null)
            {
                dynamic f = filters;
                if (f.clientId != null)
                {
                    sb.Append(" AND o.client_id = @clientId");
                    cmd.Parameters.AddWithValue("@clientId", f.clientId);
                }
                if (f.orderId != null)
                {
                    sb.Append(" AND o.order_id = @orderId");
                    cmd.Parameters.AddWithValue("@orderId", f.orderId);
                }
                if (f.dateFrom != null)
                {
                    sb.Append(" AND o.order_date >= @df");
                    cmd.Parameters.AddWithValue("@df", f.dateFrom);
                }
                if (f.dateTo != null)
                {
                    sb.Append(" AND o.order_date <= @dt");
                    cmd.Parameters.AddWithValue("@dt", f.dateTo);
                }
                if (!string.IsNullOrWhiteSpace((string?)f.estado))
                {
                    sb.Append(" AND o.estado = @est");
                    cmd.Parameters.AddWithValue("@est", f.estado);
                }
            }

            cmd.CommandText = sb.ToString();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Invoice
                {
                    OrderId = rdr.GetInt32(0),
                    OrderDate = rdr.GetDateTime(1),
                    Estado = rdr.GetString(2),
                    ClientId = rdr.GetInt32(3),
                    CompanyName = rdr.GetString(4)
                });
            }

            return list;
        }
    }
}

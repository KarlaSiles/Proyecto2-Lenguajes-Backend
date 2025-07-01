using Mercatika.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.Domain;

namespace Mercatika.Business
{
    public class ReportBusiness
    {
        private readonly ReportData _repo;
        public ReportBusiness(string conn) => _repo = new ReportData(conn);

        public Task<IEnumerable<Invoice>> GetAllAsync()
            => _repo.GetAllInvoicesAsync();

        public Task<IEnumerable<Invoice>> GetFilteredAsync(
            int? clientId,
            int? orderId,
            DateTime? dateFrom,
            DateTime? dateTo,
            string? estado)
            => _repo.GetFilteredInvoicesAsync(clientId, orderId, dateFrom, dateTo, estado);

        public Task<Invoice> GetByOrderIdAsync(int orderId)
            => _repo.GetInvoiceByOrderIdAsync(orderId);
    }
}

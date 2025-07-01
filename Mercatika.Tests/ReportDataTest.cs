using Mercatika.DataAccess;
using Mercatika.Domain;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mercatika.Test
{
    public class ReportDataTests
    {
        private ReportData reportData;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=163.178.173.130;Database=Mercatika_Proyecto2_Lenguajes;User Id=Lenguajes;Password=lenguajesparaiso2025;TrustServerCertificate=True;";
            reportData = new ReportData(connectionString);
        }

        [Test]
        public async Task GetAllInvoices_ReturnsNonEmptyList()
        {
            IEnumerable<Invoice> invoices = await reportData.GetAllInvoicesAsync();

            Assert.That(invoices, Is.Not.Null);
            Assert.That(invoices.Any(), Is.True, "No se obtuvieron facturas en el listado general.");
        }

        [Test]
        public async Task GetFilteredInvoices_ByClientIdAndEstado_ReturnsExpected()
        {
            //ajustar valores según datos de prueba
            int clienteExistente = 1;
            string estadoExistente = "pagado";

            IEnumerable<Invoice> filtered = await reportData.GetFilteredInvoicesAsync(
                clientId: clienteExistente,
                orderId: null,
                dateFrom: null,
                dateTo: null,
                estado: estadoExistente
            );

            Assert.That(filtered, Is.Not.Null);
            Assert.That(filtered.All(i => i.ClientId == clienteExistente), "Se devolvió alguna factura de otro cliente.");
            Assert.That(filtered.All(i => i.Estado == estadoExistente), "Se devolvió alguna factura con estado distinto.");
        }

        [Test]
        public async Task GetInvoiceByOrderId_ValidOrder_ReturnsCompleteInvoice()
        {
            //ajustar este orderId a uno que exista y tenga líneas
            int orderIdExistente = 10;

            Invoice inv = await reportData.GetInvoiceByOrderIdAsync(orderIdExistente);

            Assert.That(inv, Is.Not.Null);
            Assert.That(inv.OrderId, Is.EqualTo(orderIdExistente));
            Assert.That(inv.Lines, Is.Not.Null.And.Not.Empty, "La factura debería tener líneas de detalle.");
            Assert.That(inv.Subtotal, Is.GreaterThan(0m), "El subtotal debería ser mayor que cero.");
            Assert.That(inv.TaxAmount, Is.GreaterThanOrEqualTo(0m), "El impuesto debería ser >= 0.");
            Assert.That(inv.Total, Is.EqualTo(inv.Subtotal + inv.TaxAmount));
        }

        [Test]
        public void GetInvoiceByOrderId_InvalidOrder_ThrowsKeyNotFoundException()
        {
            int orderIdNoExistente = -1;

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await reportData.GetInvoiceByOrderIdAsync(orderIdNoExistente);
            }, "Debería lanzarse KeyNotFoundException si la orden no existe.");
        }
    }
}

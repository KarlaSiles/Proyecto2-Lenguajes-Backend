using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mercatika.Domain;
using Mercatika.DataAccess;
using System.Configuration;
using System.Collections.Generic;

namespace Mercatika.Tests
{
    [TestClass]
    public class ClientDataTest
    {
        private readonly string _connectionString;
        private readonly ClientData _clientData;

        public ClientDataTest()
        {
            _connectionString = "TuCadenaDeConexionAqui";
            _clientData = new ClientData(_connectionString);
        }

        [TestMethod]
        public void GetById_ShouldReturnClient_WhenClientExists()
        {
            // Arrange
            int existingClientId = 3; 

            // Act
            var client = _clientData.GetById(existingClientId);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(client);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(existingClientId, client.ClientId);
        }

        [TestMethod]
        public void GetById_ShouldReturnNull_WhenClientNotExists()
        {
            // Arrange
            int nonExistingClientId = 9999;

            // Act
            var client = _clientData.GetById(nonExistingClientId);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(client);
        }

        [TestMethod]
        public void GetByName_ShouldReturnClients_WhenNameMatches()
        {
            // Arrange
            string searchName = "Ana"; 

            // Act
            var clients = _clientData.GetByName(searchName);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(clients);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(clients.Count > 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(clients.Exists(c => c.ContractName.Contains(searchName)));
        }

        [TestMethod]
        public void GetByCompanyName_ShouldReturnClients_WhenCompanyMatches()
        {
            // Arrange
            string searchCompany = "TechCorp"; 

            // Act
            var clients = _clientData.GetByCompanyName(searchCompany);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(clients);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(clients.Count > 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(clients.Exists(c => c.CompanyName.Contains(searchCompany)));
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllClients()
        {
            // Act
            var clients = _clientData.GetAll();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(clients);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(clients.Count > 0); 
        }

        [TestMethod]
        public void Insert_ShouldAddNewClient_AndReturnId()
        {
            // Arrange
            var newClient = new Client
            {
                CompanyName = "Test Company",
                ContractName = "Test",
                ContractLastname = "User",
                ContractPosition = "Tester",
                Address = "123 Test St",
                City = "Testville",
                Province = "Testland",
                ZipCode = 12345,
                Country = "Testonia",
                Phone = 1234567890,
                FaxNumber = 1234567891
            };

            // Act
            int newId = _clientData.Insert(newClient);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(newId > 0);

            // Cleanup (opcional)
            _clientData.Delete(newId);
        }

        [TestMethod]
        public void Update_ShouldModifyExistingClient()
        {
            // Arrange
            int existingClientId = 4;
            var originalClient = _clientData.GetById(existingClientId);
            var updatedClient = new Client
            {
                ClientId = existingClientId,
                CompanyName = originalClient.CompanyName + " Updated",
                ContractName = originalClient.ContractName,
                ContractLastname = originalClient.ContractLastname,
                ContractPosition = originalClient.ContractPosition,
                Address = originalClient.Address,
                City = originalClient.City,
                Province = originalClient.Province,
                ZipCode = originalClient.ZipCode,
                Country = originalClient.Country,
                Phone = originalClient.Phone,
                FaxNumber = originalClient.FaxNumber
            };

            // Act
            bool success = _clientData.Update(updatedClient);
            var clientAfterUpdate = _clientData.GetById(existingClientId);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(success);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(updatedClient.CompanyName, clientAfterUpdate.CompanyName);

            // Restaurar valores originales (opcional)
            _clientData.Update(originalClient);
        }

        [TestMethod]
        public void Delete_ShouldRemoveClient()
        {
            // Arrange - Primero creamos un cliente para eliminar
            var testClient = new Client
            {
                CompanyName = "Client to Delete",
                ContractName = "Delete",
                ContractLastname = "Me",
                ContractPosition = "Position",
                Address = "Address",
                City = "City",
                Province = "Province",
                ZipCode = 12345,
                Country = "Country",
                Phone = 1234567890,
                FaxNumber = 1234567890
            };
            int clientId = _clientData.Insert(testClient);

            // Act
            bool deleteSuccess = _clientData.Delete(clientId);
            var clientAfterDelete = _clientData.GetById(clientId);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(deleteSuccess);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(clientAfterDelete);
        }
    }
}

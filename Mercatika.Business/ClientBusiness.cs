using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercatika.DataAccess;
using Mercatika.Domain;

namespace Mercatika.Business
{
    public class ClientBusiness
    {
        private readonly ClientData _clientData;

        public ClientBusiness(string connectionString)
        {
            _clientData = new ClientData(connectionString);
        }

        public int CreateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client object cannot be null");
            }

            if (string.IsNullOrWhiteSpace(client.CompanyName))
                throw new ArgumentException("Company name is required");

            if (string.IsNullOrWhiteSpace(client.ContractName))
                throw new ArgumentException("Contract name is required");

            if (string.IsNullOrWhiteSpace(client.ContractLastname))
                throw new ArgumentException("Contract lastname is required");

            return _clientData.Insert(client);
        }

        public bool UpdateClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Client object cannot be null");
            }

            if (client.ClientId <= 0)
            {
                throw new ArgumentException("Invalid client ID");
            }

            if (string.IsNullOrWhiteSpace(client.CompanyName))
                throw new ArgumentException("Company name is required");

            if (string.IsNullOrWhiteSpace(client.ContractName))
                throw new ArgumentException("Contract name is required");

            return _clientData.Update(client);
        }

        public bool DeleteClient(int clientId)
        {
            if (clientId <= 0)
            {
                throw new ArgumentException("Invalid client ID");
            }

            return _clientData.Delete(clientId);
        }

        public List<Client> GetClientsByCompanyName(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
            {
                throw new ArgumentException("Company name cannot be empty");
            }

            return _clientData.GetByCompanyName(companyName);
        }

        public Client GetClientById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid client ID");
            }

            return _clientData.GetById(id);
        }

        public List<Client> GetClientsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }

            return _clientData.GetByName(name);
        }

        public List<Client> GetClientsByLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
            {
                throw new ArgumentException("Lastname cannot be empty");
            }

            return _clientData.GetByLastname(lastname);
        }

        public List<Client> GetAllClients()
        {
            return _clientData.GetAll();
        }
    }
}

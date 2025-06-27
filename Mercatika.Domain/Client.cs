namespace Mercatika.Domain
{
    public class Client
    {
        private int clientId;
        private string companyName;
        private string contractName;
        private string contractLastname;
        private string contractPosition;
        private string address;
        private string city;
        private string province;
        private int zipCode;
        private string country;
        private int phone;
        private int faxNumber;

        public Client()
        {
        }

        public Client(int clientId, string companyName, string contractName, string contractLastname, string contractPosition,
                      string address, string city, string province, int zipCode, string country, int phone, int faxNumber)
        {
            this.clientId = clientId;
            this.companyName = companyName;
            this.contractName = contractName;
            this.contractLastname = contractLastname;
            this.contractPosition = contractPosition;
            this.address = address;
            this.city = city;
            this.province = province;
            this.zipCode = zipCode;
            this.country = country;
            this.phone = phone;
            this.faxNumber = faxNumber;
        }

        public int ClientId
        {
            get => clientId;
            set => clientId = value;
        }

        public string CompanyName
        {
            get => companyName;
            set => companyName = value;
        }

        public string ContractName
        {
            get => contractName;
            set => contractName = value;
        }

        public string ContractLastname
        {
            get => contractLastname;
            set => contractLastname = value;
        }

        public string ContractPosition
        {
            get => contractPosition;
            set => contractPosition = value;
        }

        public string Address
        {
            get => address;
            set => address = value;
        }

        public string City
        {
            get => city;
            set => city = value;
        }

        public string Province
        {
            get => province;
            set => province = value;
        }

        public int ZipCode
        {
            get => zipCode;
            set => zipCode = value;
        }

        public string Country
        {
            get => country;
            set => country = value;
        }

        public int Phone
        {
            get => phone;
            set => phone = value;
        }

        public int FaxNumber
        {
            get => faxNumber;
            set => faxNumber = value;
        }
    }
}

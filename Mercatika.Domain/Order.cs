namespace Mercatika.Domain
{
    public class Order
    {
        private int orderId;
        private int clientId;
        private int employeeId;
        private DateTime orderDate;
        private string addressTrip;
        private string provinceTrip;
        private string countryTrip;
        private int phoneTrip;
        private DateTime dateTrip;

        public Order()
        {
        }

        public Order(int orderId, int clientId, int employeeId, DateTime orderDate, string addressTrip, string provinceTrip, string countryTrip, int phoneTrip, DateTime dateTrip)
        {
            this.orderId = orderId;
            this.clientId = clientId;
            this.employeeId = employeeId;
            this.orderDate = orderDate;
            this.addressTrip = addressTrip;
            this.provinceTrip = provinceTrip;
            this.countryTrip = countryTrip;
            this.phoneTrip = phoneTrip;
            this.dateTrip = dateTrip;
        }

        public int OrderId
        {
            get => orderId;
            set => orderId = value;
        }

        public int ClientId
        {
            get => clientId;
            set => clientId = value;
        }

        public int EmployeeId
        {
            get => employeeId;
            set => employeeId = value;
        }

        public DateTime OrderDate
        {
            get => orderDate;
            set => orderDate = value;
        }

        public string AddressTrip
        {
            get => addressTrip;
            set => addressTrip = value;
        }

        public string ProvinceTrip
        {
            get => provinceTrip;
            set => provinceTrip = value;
        }

        public string CountryTrip
        {
            get => countryTrip;
            set => countryTrip = value;
        }

        public int PhoneTrip
        {
            get => phoneTrip;
            set => phoneTrip = value;
        }

        public DateTime DateTrip
        {
            get => dateTrip;
            set => dateTrip = value;
        }
    }
}

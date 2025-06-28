using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercatika.Domain
{
    public class Employee
    {
        private int employeeId;
        private string employeeName;
        private string employeeLastname;
        private string position;
        private int extention;
        private int workPhone;
        private int deptoCode;
        private int roleId;

        public Employee()
        {
        }

        public Employee(int employeeId, string employeeName, string employeeLastname, string position, int extention, int workPhone, int deptoCode, int roleId)
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
            this.employeeLastname = employeeLastname;
            this.position = position;
            this.extention = extention;
            this.workPhone = workPhone;
            this.deptoCode = deptoCode;
            this.roleId = roleId;
        }

        public int EmployeeId
        {
            get => employeeId;
            set => employeeId = value;
        }

        public string EmployeeName
        {
            get => employeeName;
            set => employeeName = value;
        }

        public string EmployeeLastname
        {
            get => employeeLastname;
            set => employeeLastname = value;
        }

        public string Position
        {
            get => position;
            set => position = value;
        }

        public int Extention
        {
            get => extention;
            set => extention = value;
        }

        public int WorkPhone
        {
            get => workPhone;
            set => workPhone = value;
        }

        public int DeptoCode
        {
            get => deptoCode;
            set => deptoCode = value;
        }

        public int RoleId
        {
            get => roleId;
            set => roleId = value;
        }
    }
}

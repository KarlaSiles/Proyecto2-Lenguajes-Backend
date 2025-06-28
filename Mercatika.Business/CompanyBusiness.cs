using Mercatika.DataAccess;
using Mercatika.Domain;

namespace Mercatika.Business
{
    public class CompanyBusiness
    {
        private readonly CompanyData _companyData;

        public CompanyBusiness(string connectionString)
        {
            _companyData = new CompanyData(connectionString);
        }

        public bool UpdateCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company), "Company object cannot be null");
            }

            if (company.Idsetup <= 0)
            {
                throw new ArgumentException("Invalid company ID");
            }

            if (string.IsNullOrWhiteSpace(company.Name_company))
                throw new ArgumentException("Company name is required");

            return _companyData.Update(company);
        }

        public Company GetCompany()
        {
            return _companyData.GetById(1); // Asumiendo que el ID de la compañía es 1
        }
    }
}
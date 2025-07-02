using System.Threading.Tasks;
using Proyecto.DataAccess;

namespace Proyecto.Business
{
    public class LoginBusiness
    {
        private readonly LoginData _repo;
        public LoginBusiness(string conn) => _repo = new LoginData(conn);

        /// <summary>
        /// Valida las credenciales. En un escenario real aquí aplicarías hashing/salting.
        /// </summary>
        public async Task<bool> ValidateAsync(string username, string password)
        {
            var login = await _repo.GetByUsernameAsync(username);
            if (login is null)
                return false;

            return login.Password == password;
        }
    }
}

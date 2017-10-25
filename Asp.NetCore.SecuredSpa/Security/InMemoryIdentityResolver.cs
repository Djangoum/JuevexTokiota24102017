using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCore.SecuredSpa.Security
{
    public class InMemoryIdentityResolver : IIdentityResolver
    {
        private static LoginModel[] registeredTenants = null;

        public InMemoryIdentityResolver()
        {
            if (registeredTenants == null)
            {
                registeredTenants = new LoginModel[2];
                registeredTenants[0] = new LoginModel()
                {
                    Username = "ariel",
                    Password = "1234"
                };
                registeredTenants[1] = new LoginModel()
                {
                    Username = "aman1",
                    Password = "test123"
                };
            }
        }

        public bool IsIdentityConfirmed(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return registeredTenants.Count(x => x.Username == username &&
                                                x.Password == password) == 1;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

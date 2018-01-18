using Chiron.Auth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.Services
{
    public interface IAuthenticator
    {
	Task<User> AuthenticateAsync(LoginInfo info);
    }
}

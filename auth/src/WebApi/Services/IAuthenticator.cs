using System.Threading.Tasks;
using Chiron.Auth.Data;
using Chiron.Auth.WebApi.Data;

namespace Chiron.Auth.WebApi.Services {
    public interface IAuthenticator {
        Task<User> AuthenticateAsync(LoginInfo info);
    }
}

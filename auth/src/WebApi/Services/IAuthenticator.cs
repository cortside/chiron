using System.Threading.Tasks;
using Chiron.Auth.Data;

namespace Chiron.Auth.Services {
    public interface IAuthenticator {
        Task<User> AuthenticateAsync(LoginInfo info);
    }
}

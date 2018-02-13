using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.Data {
    /// <summary>
    /// Interface for User DB Context.
    /// </summary>
    public interface IUserDbContext : IDisposable {
        void AddUser(User user);
        void AddRole(Role role);

        Task SaveChangesAsync();

        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
    }
}

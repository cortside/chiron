using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chiron.Auth.WebApi.Data {
    /// <summary>
    /// Interface for User DB Context.
    /// </summary>
    public interface IUserDbContext : IDisposable {
        void AddUser(User user);
        void AddRole(Role role);

        Task SaveChangesAsync();
        void SaveChanges();

        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
    }
}

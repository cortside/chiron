using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chiron.Auth.WebApi.Data {
    public class UserDbContext : DbContext, IUserDbContext {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        //TODO: Move these registrations into their own mapping classes as in prior versions of EF.
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("auth");
            modelBuilder
            .Entity<User>(x => { x.ToTable("User"); });
            modelBuilder
             .Entity<Role>(x => { x.ToTable("Role"); });
            modelBuilder
            .Entity<UserRole>(x => {
                x.ToTable("UserRole").HasKey(ur => new { ur.UserId, ur.RoleId });
                x.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
                x.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            });
        }

        public DbSet<User> Users { set; get; }
        public DbSet<Role> Roles { set; get; }
        public DbSet<UserRole> UserRoles { set; get; }
        IQueryable<User> IUserDbContext.Users { get { return Users; } }
        IQueryable<Role> IUserDbContext.Roles { get { return Roles; } }

        public void AddUser(User user) {
            Users.Add(user);
        }

        public void AddRole(Role role) {
            Roles.Add(role);
        }

        public async Task SaveChangesAsync() {
            await base.SaveChangesAsync();
        }
    }
}

namespace Chiron.Auth.Data {
    public interface IUserDbContextFactory {
        IUserDbContext NewUserDbContext();
    }
}

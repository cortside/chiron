namespace Chiron.Auth.WebApi.Data {
    public interface IUserDbContextFactory {
        IUserDbContext NewUserDbContext();
    }
}

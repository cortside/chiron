namespace Chiron.Auth.WebApi.Services {
    public interface IHashProvider {
        string ComputeHash(string theString);
    }
}

namespace Chiron.Auth.Services {
    public interface IHashProvider {
        string ComputeHash(string theString);
    }
}

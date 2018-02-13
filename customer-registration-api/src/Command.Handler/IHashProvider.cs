namespace Chiron.Registration.Customer.Command.Handler {
    public interface IHashProvider {
        string ComputeHash(string theString);
    }
}

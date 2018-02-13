using Cortside.Common.Command;

namespace Chiron.Registration.Customer.Customer.Command {
    public class CreateCustomerCommand : ICommand {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string PhoneNumber { set; get; }
        public bool AgreeToTerms { set; get; }
    }
}

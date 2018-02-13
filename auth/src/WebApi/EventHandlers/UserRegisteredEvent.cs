using System;

namespace Chiron.Auth.EventHandlers {
    public class UserRegisteredEvent {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string Salt { set; get; }
        public string PhoneNumber { set; get; }
        public DateTime AgreeToTerms { set; get; }
        public string Role { set; get; }
    }
}

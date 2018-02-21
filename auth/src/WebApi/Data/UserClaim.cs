using System;

namespace Chiron.Auth.WebApi.Data {
    //Limitation of EFCore 1.1 - This class should not exist.
    public class UserClaim {
        public int UserClaimId { set; get; }
        public Guid UserId { set; get; }
        public User User { set; get; }
        public string ProviderName { set; get; }
        public string Type { set; get; }
        public string Value { set; get; }
    }
}

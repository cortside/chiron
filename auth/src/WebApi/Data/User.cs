using System;
using System.Collections.Generic;

namespace Chiron.Auth.WebApi.Data {
    public class User {
        public Guid UserId { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public string Salt { set; get; }
        public string UserStatus { set; get; } //TODO: Want to map this to an enum some how. Need to look into this.
        public DateTime EffectiveDate { set; get; }
        public DateTime? ExpirationDate { set; get; }
        public DateTime? LastLogin { set; get; }
        public string LastLoginIPAddress { set; get; }
        public DateTime TermsOfUseAcceptanceDate { get; set; }

        public Guid CreateUserId { set; get; }
        public DateTime CreateDate { set; get; }
        public Guid LastModifiedUserId { set; get; }
        public DateTime LastModifiedDate { set; get; }

        public ICollection<UserRole> UserRoles { get; set; } //This should be Roles, not UserRoles, but EFCore cannot handle this right now.
        public string ProviderName { get; internal set; }
        public string ProviderSubjectId { get; internal set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}

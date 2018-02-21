using System;
using System.Collections.Generic;

namespace Chiron.Auth.WebApi.Data {
    public class Role {
        public int RoleId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public ICollection<UserRole> UserRoles { get; set; }

        public Guid CreateUserId { set; get; }
        public DateTime CreateDate { set; get; }
        public Guid LastModifiedUserId { set; get; }
        public DateTime LastModifiedDate { set; get; }
    }
}

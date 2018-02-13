using System;
using System.Collections.Generic;

namespace Chiron.Auth.Data {
    public class Role {
        public int RoleId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public ICollection<UserRole> UserRoles { get; set; }

        public int CreateUserId { set; get; }
        public DateTime CreateDate { set; get; }
        public int LastModifiedUserId { set; get; }
        public DateTime LastModifiedDate { set; get; }
    }
}

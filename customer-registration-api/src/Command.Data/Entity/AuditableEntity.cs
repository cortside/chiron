using System;

namespace Chiron.Registration.Customer.Command.Data.Entity {

    public class AuditableEntity : IAuditableEntity {

        // audit stamp
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedBy { get; set; }
    }
}

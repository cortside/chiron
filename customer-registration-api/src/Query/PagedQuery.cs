using System;
using Cortside.Common.Query;

namespace Chiron.Registration.Customer.Query {

    public class PagedQuery : IQuery {
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
    }
}

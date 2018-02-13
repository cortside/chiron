using System.Collections.Generic;
using Cortside.Common.Query;
namespace Chiron.Registration.Customer.Query.Data {
    public class ListResult<T> : IQueryResult {
        public List<T> Results { get; set; }
    }
}

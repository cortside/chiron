using System.Collections.Generic;
using Cortside.Common.Query;
namespace Chiron.Admin.Query.Data {
    public class ListResult<T> : IQueryResult {
        public List<T> Results { get; set; }
    }
}

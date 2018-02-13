using System.Collections.Generic;
using Cortside.Common.Query;

namespace Chiron.Admin.Query.Model {

    public class PagedResult<T> : IQueryResult {
        public List<T> Results { get; set; }
        public int Count { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }
    }
}

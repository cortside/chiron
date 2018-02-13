using Cortside.Common.Query;
using System.Collections.Generic;

namespace Chiron.Catalog.Query.Model {

    public class PagedResult<T> : IQueryResult {
	public List<T> Results { get; set; }
	public int Count { get; set; }

	public int Page { get; set; }

	public int PageSize { get; set; }

	public string Sort { get; set; }
    }
}

using Cortside.Common.Query;
using System.Collections.Generic;
namespace Chiron.Catalog.Query.Data
{
    public class ListResult<T> : IQueryResult
    {
	public List<T> Results { get; set; }
    }
}

using Cortside.Common.Query;

namespace Chiron.Catalog.Query.Data {

    public class ScalarResult<T> : IQueryResult {
	public T Result { get; set; }
    }
}

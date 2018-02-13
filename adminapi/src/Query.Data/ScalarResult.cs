using Cortside.Common.Query;

namespace Chiron.Admin.Query.Data {

    public class ScalarResult<T> : IQueryResult {
        public T Result { get; set; }
    }
}

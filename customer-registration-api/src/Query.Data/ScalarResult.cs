using Cortside.Common.Query;

namespace Chiron.Registration.Customer.Query.Data {

    public class ScalarResult<T> : IQueryResult {
        public T Result { get; set; }
    }
}

using System.Data.SqlClient;
using Cortside.Common.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chiron.Registration.Customer.Command.Data {

    public class DatabaseManager {
        private static DatabaseManager _instance;

        private DatabaseManager() {
        }

        static DatabaseManager() {
            _instance = new DatabaseManager();
        }

        public static DatabaseManager Instance => _instance;

        private string GetConnectionString() {
            IConfigurationRoot configuration = DI.Container.GetService<IConfigurationRoot>();

            return configuration.GetSection("SQL")["ConnetionString"];
            //return configuration.Get<string>("SQL:ConnectionString");
        }

        public SqlConnection CreateSqlConnection() => new SqlConnection(GetConnectionString());
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cortside.Common.IoC;
using System.Data.SqlClient;

namespace Chiron.Catalog.Query.Data.Dao {

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
	    return configuration.GetSection("SQL")["ConnectionString"];
	}

	public SqlConnection CreateSqlConnection() => new SqlConnection(GetConnectionString());
    }
}

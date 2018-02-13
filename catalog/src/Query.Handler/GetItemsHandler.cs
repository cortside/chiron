using Chiron.Catalog.Command.Model.ViewModels;
using Chiron.Catalog.Query.Data.Dao;
using Chiron.Catalog.Query.Model;
using Cortside.Common.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Chiron.Catalog.Query.Handler {
    public class GetItemsHandler : IQueryHandler<GetItemsQuery, IEnumerable<SimpleItemModel>> {

        public async Task<IEnumerable<SimpleItemModel>> Retrieve(GetItemsQuery query) {
            using (var conn = DatabaseManager.Instance.CreateSqlConnection()) {
                using (var cmd = conn.CreateCommand()) {
                    await conn.OpenAsync();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from [Catalog].[Item]";

                    // TODO use dapper

                    IList<SimpleItemModel> results = new List<SimpleItemModel>();
                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync()) {
                        var result = new SimpleItemModel();
                        result.ItemId = Convert.ToInt32(reader["ItemId"]);
                        result.Description = reader["Description"].ToString();
                        result.Title = reader["Title"].ToString();

                        results.Add(result);
                    }
                    return results;
                }
            }
        }
    }
}

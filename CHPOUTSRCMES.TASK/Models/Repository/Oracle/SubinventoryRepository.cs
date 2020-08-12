using CHPOUTSRCMES.TASK.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace CHPOUTSRCMES.TASK.Models.Repository.Oracle
{
    public class SubinventoryRepository : GenericRepository<XXCINV_SUBINVENTORY_V>
    {
        public SubinventoryRepository(IDbConnection conn, string tableName) : base(conn, tableName)
        {

        }

        public async Task<IEnumerable<XXCINV_SUBINVENTORY_V>> GetByLastUpdateDate(DateTime lastUpdatedDate)
        {
            var param = new DynamicParameters();
            param.Add(":lastUpdatedDate", lastUpdatedDate, DbType.DateTime2, ParameterDirection.Input);
            string cmd = $@"{GenerateSelectQuery()} A WHERE LAST_UPDATED_DATE > :lastUpdatedDate";


            return await Connection.QueryAsync<XXCINV_SUBINVENTORY_V>(cmd, param);
        }

    }
}

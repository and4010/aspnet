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
    public class ItemRepository : GenericRepository<XXIFV050_ITEMS_FTY_V>
    {
        public ItemRepository(IDbConnection conn, string tableName) : base(conn, tableName)
        {
        }


        public async Task<IEnumerable<XXIFV050_ITEMS_FTY_V>> GetRangeAsync(long skip, long take)
        {
            string cmd = 
$@"SELECT * FROM ( 
SELECT A.*, ROWNUM rnum FROM ({GenerateSelectQuery()} ORDER BY INVENTORY_ITEM_ID) A WHERE ROWNUM <= {(skip + take)}
) B WHERE rnum > {skip}";


            return await Connection.QueryAsync<XXIFV050_ITEMS_FTY_V>(cmd);
        }

        public async Task<DateTime> GetLastUpdateDateAsync()
        {
            string cmd =
$@"SELECT MAX(LAST_UPDATE_DATE) FROM XXIFV050_ITEMS_FTY_V";

            return await Connection.QuerySingleOrDefaultAsync<DateTime>(cmd);

        }

        public async Task<IEnumerable<XXIFV050_ITEMS_FTY_V>> GetAllByLastUpdateDate(DateTime date)
        {
            string cmd =
$@"{GenerateSelectQuery()} WHERE LAST_UPDATE_DATE > :date1 ORDER BY INVENTORY_ITEM_ID ";
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":date1", date, DbType.Date);

            return await Connection.QueryAsync<XXIFV050_ITEMS_FTY_V>(cmd, parameters);
        }

        public async Task<long> CountByLastUpdateDateAsync(DateTime date)
        {
            string cmd =
$@"SELECT COUNT(*) FROM {tableName} A WHERE LAST_UPDATE_DATE > :date1";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":date1", date, DbType.Date);

            return await Connection.QuerySingleAsync<long>(cmd, parameters);
        }

    }
}

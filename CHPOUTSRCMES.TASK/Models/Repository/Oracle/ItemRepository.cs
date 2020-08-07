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


        public async Task<IEnumerable<XXIFV050_ITEMS_FTY_V>> GetRangeAsync(int skip, int take)
        {
            string cmd = 
$@"SELECT * FROM ( 
SELECT A.*, ROWNUM rnum FROM ({GenerateSelectQuery()} ORDER BY INVENTORY_ITEM_ID) A WHERE ROWNUM <= {(skip + take).ToString()}
) B WHERE rnum > {skip.ToString()}";


            return await Connection.QueryAsync<XXIFV050_ITEMS_FTY_V>(cmd);
        }

    }
}

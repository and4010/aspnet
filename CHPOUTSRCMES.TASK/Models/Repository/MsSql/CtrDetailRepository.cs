using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CHPOUTSRCMES.TASK.Models.Entity;
using Dapper;


namespace CHPOUTSRCMES.TASK.Models.Repository.MsSql
{
    public class CtrDetailRepository : GenericRepository<CTR_DETAIL_T>
    {
        
        #region Constructor
        public CtrDetailRepository()
        {
            IdField = "CTR_DETAIL_ID";
        }

        public CtrDetailRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "CTR_DETAIL_ID";
        }

        #endregion


        public async Task<List<CTR_DETAIL_T>> GetFlatListBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            var cmd = 
$@"SELECT B.*
FROM XXIF_CHP_CONTROL_ST A
JOIN XXIF_CHP_P217_CONTAINER_ST C ON C.PROCESS_CODE = A.PROCESS_CODE AND C.SERVER_CODE = A.SERVER_CODE AND C.BATCH_ID = A.BATCH_ID
JOIN CTR_DETAIL_T B ON B.PROCESS_CODE = C.PROCESS_CODE AND B.SERVER_CODE = C.SERVER_CODE AND B.BATCH_ID = C.BATCH_ID AND B.BATCH_LINE_ID = C.BATCH_LINE_ID
WHERE A.PROCESS_CODE = @processCode AND A.SERVER_CODE = @serverCode AND A.BATCH_ID = @batchId AND B.ITEM_CATEGORY = @itemCategory";

            var p = new DynamicParameters();
            p.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
            p.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
            p.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
            p.Add(name: "@itemCategory", value: "平版", dbType: DbType.String, direction: ParameterDirection.Input, size: 10);
            return (await Connection.QueryAsync<CTR_DETAIL_T>(sql: cmd, param: p, transaction: transaction)).ToList();
        }
        #region IDispose Region
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}

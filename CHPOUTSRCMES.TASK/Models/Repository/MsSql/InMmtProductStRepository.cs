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
    public class InMmtProductStRepository : GenericRepository<XXIF_CHP_P211_IN_MMT_PROD_ST>
    {
        
        #region Constructor
        public InMmtProductStRepository()
        {
            IdField = "XXIF_CHP_P211_IN_MMT_PROD_ST";
        }

        public InMmtProductStRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "XXIF_CHP_P211_IN_MMT_PROD_ST";
        }

        #endregion

        public long GetInventoryItemId(string itemNo, IDbTransaction transaction = null)
        {
            return Connection.Query<long>(
@"SELECT INVENTORY_ITEM_ID FROM ITEMS_T WHERE ITEM_NUMBER = @itemNo", new { itemNo = itemNo }, transaction: transaction).FirstOrDefault();
        }


        public List<XXIF_CHP_P211_IN_MMT_PROD_ST> GetListBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return Connection.Query<XXIF_CHP_P211_IN_MMT_PROD_ST>(
@"SELECT * FROM XXIF_CHP_P211_IN_MMT_PROD_ST A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SERVER_CODE = @ServerCode AND A.BATCH_ID = @BatchId
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID, A.BATCH_LINE_ID", new { ProcessCode = processCode, ServerCode = serverCode, BatchId = batchId }, transaction: transaction).ToList();
        }

        public int Update(XXIF_CHP_P211_IN_MMT_PROD_ST entity, IDbTransaction transaction = null)
        {
            return Connection.Execute(
@"UPDATE dbo.XXIF_CHP_P211_IN_MMT_PROD_ST SET 
STATUS_CODE = @STATUS_CODE
, ERROR_MSG = @ERROR_MSG
, TRANSACTION_UOM = @TRANSACTION_UOM
, TRANSACTION_QUANTITY = @TRANSACTION_QUANTITY
, SECONDARY_UOM_CODE = @SECONDARY_UOM_CODE
, SECONDARY_TRANSACTION_QUANTITY = @SECONDARY_TRANSACTION_QUANTITY
WHERE PROCESS_CODE = @PROCESS_CODE
	AND SERVER_CODE = @SERVER_CODE
	AND BATCH_ID = @BATCH_ID
	AND BATCH_LINE_ID = @BATCH_LINE_ID", entity, transaction);
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

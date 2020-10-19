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
    public class ShipConfirmStRepository : GenericRepository<XXIF_CHP_P221_SHIP_CONFIRM_ST>
    {
        
        #region Constructor
        public ShipConfirmStRepository()
        {
            IdField = "XXIF_CHP_P221_SHIP_CONFIRM_ST";
        }

        public ShipConfirmStRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "XXIF_CHP_P221_SHIP_CONFIRM_ST";
        }

        #endregion

        public List<XXIF_CHP_P221_SHIP_CONFIRM_ST> GetListBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return Connection.Query<XXIF_CHP_P221_SHIP_CONFIRM_ST>(
@"SELECT * FROM XXIF_CHP_P221_SHIP_CONFIRM_ST A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SERVER_CODE = @ServerCode AND A.BATCH_ID = @BatchId
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID, A.BATCH_LINE_ID", new { ProcessCode = processCode, ServerCode = serverCode, BatchId = batchId }, transaction: transaction).ToList();
        }

        public int Update(XXIF_CHP_P221_SHIP_CONFIRM_ST entity, IDbTransaction transaction = null)
        {
            return Connection.Execute(
@"UPDATE dbo.XXIF_CHP_P221_SHIP_CONFIRM_ST SET 
STATUS_CODE = @STATUS_CODE
, ERROR_MSG = @ERROR_MSG
, TRANSACTION_UOM = @TRANSACTION_UOM
, TRANSACTION_QUANTITY = @TRANSACTION_QUANTITY
, PRIMARY_UOM = @PRIMARY_UOM
, PRIMARY_QUANTITY = @PRIMARY_QUANTITY
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

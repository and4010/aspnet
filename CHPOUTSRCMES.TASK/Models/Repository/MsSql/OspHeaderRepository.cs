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
    public class OspHeaderRepository : GenericRepository<OSP_HEADER_T>
    {
        
        #region Constructor
        public OspHeaderRepository()
        {
            IdField = "OSP_HEADER_ID";
        }

        public OspHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "OSP_HEADER_ID";
        }

        #endregion

        public async Task<List<long>> GetStage1List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND (S1.OSP_HEADER_ID IS NULL OR S1.STATUS_CODE = 'R')
", transaction: transaction)).ToList();
        }

        public async Task<List<long>> GetExportedStage1List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND S1.STATUS_CODE IS NULL)
", transaction: transaction)).ToList();
        }

        public async Task<List<long>> GetStage2List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S2_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND S1.OSP_HEADER_ID IS NOT NULL AND S1.STATUS_CODE = 'S'
AND (S2.OSP_HEADER_ID IS NULL OR S2.STATUS_CODE = 'R')
", transaction: transaction)).ToList();
        }

        public async Task<List<long>> GetExportedStage2List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S2_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND S1.OSP_HEADER_ID IS NOT NULL AND S1.STATUS_CODE = 'S'
AND S2.STATUS_CODE IS NULL)
", transaction: transaction)).ToList();
        }

        public async Task<List<long>> GetStage3List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S2_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S3_T S3 ON S3.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND S1.OSP_HEADER_ID IS NOT NULL AND S1.STATUS_CODE = 'S' 
AND S2.OSP_HEADER_ID IS NOT NULL AND S2.STATUS_CODE = 'S'
AND (S3.OSP_HEADER_ID IS NULL OR S3.STATUS_CODE = 'R')
", transaction: transaction)).ToList();
        }

        public async Task<List<long>> GetExportedStage3List(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S2_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S3_T S3 ON S3.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] = '3' 
AND S1.OSP_HEADER_ID IS NOT NULL AND S1.STATUS_CODE = 'S'
AND S2.OSP_HEADER_ID IS NOT NULL AND S2.STATUS_CODE = 'S'
AND S3.STATUS_CODE IS NULL)
", transaction: transaction)).ToList();
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

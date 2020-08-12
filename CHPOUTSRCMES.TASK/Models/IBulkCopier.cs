using System.Collections.Generic;
using System.Data.SqlClient;

namespace CHPOUTSRCMES.Web.DataModel
{
    public interface IBulkCopier<T>
    {
        void BulkCopy(SqlConnection connection, List<T> list, string tableName);
    }
}
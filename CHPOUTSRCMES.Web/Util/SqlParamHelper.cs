using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Util
{
    public class SqlParamHelper
    {
        public static SqlParameter GetBigInt(string paramName, long data, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.BigInt) { Value = data, Direction = direction };
        }

        public static SqlParameter GetInt(string paramName, int data, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.Int) { Value = data, Direction = direction };
        }

        public static SqlParameter GetDecimal(string paramName, decimal data, byte precision = 30, int scale = 10, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.Decimal) { Value = data, Precision = precision, Size = scale, Direction = direction };
        }

        public static SqlParameter GetDataTime(string paramName, DateTime data, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input) 
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.DateTime) { Value = data, Direction = direction };
        }

        public static SqlParameter GetNChar(string paramName, string data, int size = 1,System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.NChar) { Value = data, Size = size, Direction = direction };
        }

        public static SqlParameter GetNVarChar(string paramName, string data, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, data) { Value = data, Direction = direction };
        }

        public static SqlParameter GetNVarChar(string paramName, string data, int size, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.NVarChar) { Value = data, Size = size, Direction = direction };
        }

        public static SqlParameter GetVarChar(string paramName, string data, int size, System.Data.ParameterDirection direction = System.Data.ParameterDirection.Input)
        {
            return new SqlParameter(paramName, System.Data.SqlDbType.VarChar) { Value = data, Size = size, Direction = direction };
        }


        public class R
        {
            public static SqlParameter ItemNo(string paramName, string data)
            {
                return GetNVarChar(paramName, data, 40);
            }

            public static SqlParameter Barcode(string paramName, string data)
            {
                return GetNVarChar(paramName, data, 20);
            }

            public static SqlParameter SubinventoryCode(string paramName, string data)
            {
                return GetNVarChar(paramName, data, 3);
            }
        }

    }
}
using CHPOUTSRCMES.Web.DataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using NLog.Internal;
using System.Data;
using CHPOUTSRCMES.Web.Models;
using DataTables;

namespace CHPOUTSRCMES.Web.DataModel
{
    public class UomConversion : IUomConversion, IDisposable
    {
        private IDbConnection connection;

        public UomConversion()
        {
            connection = getOracleConnection();
        }


        public UomConversion(IDbConnection conn)
        {
            connection = conn;
        }

        public IDbConnection getOracleConnection()
        {
            var connStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["OracleContext"].ConnectionString;
            return new Oracle.ManagedDataAccess.Client.OracleConnection(connStr);
        }

        public ResultDataModel<decimal> Convert(long itemId, decimal fromQty, string fromUom, string toUom, int round = 5)
        {
            ResultDataModel<decimal> model = new ResultDataModel<decimal>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(":ITEM_ID", itemId);
                parameters.Add(":FROM_QTY", fromQty);
                parameters.Add(":FROM_UOM", fromUom);
                parameters.Add(":TO_UOM", toUom);
                parameters.Add(":ROUND", round);


                var data = connection.Query<decimal>(
                    "SELECT ROUND(YFY_DIS_PKG_UTIL.UOM_CONVERSION(:ITEM_ID, :FROM_QTY, :FROM_UOM, :TO_UOM), :ROUND) QUANTITY FROM DUAL", parameters
                    ).SingleOrDefault();

                model.Data = data;
                model.Success = true;
                model.Code = ResultModel.CODE_SUCCESS;
                model.Msg = "";

            }
            catch (Exception ex)
            {
                model.Code = -1;
                model.Msg = $"單位換算例外 {ex.Message ?? ""} -{ex.StackTrace ?? ""}";
                model.Data = 0m;
            }
            return model;
        }

        public void Dispose()
        { 
            try
            {
                if (this.connection != null)
                {
                    if(connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                }
            }
            finally
            {

            }

        }
    }
}
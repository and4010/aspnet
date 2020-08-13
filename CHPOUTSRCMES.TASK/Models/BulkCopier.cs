using CHPOUTSRCMES.TASK.Models.Entity.Temp;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class BulkCopier
    {

        private SqlConnection connection;

        private SqlTransaction transaction;

        public BulkCopier(SqlConnection conn, SqlTransaction trans)
        {
            this.connection = conn;
            this.transaction = trans;
        }

        private ResultModel DeleteTable(string tableName)
        {
            var resultModel = new ResultModel();

            using (var cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = $"DELETE FROM {tableName}";
                    cmd.ExecuteNonQuery();
                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Success = false;
                    resultModel.Code = -1;
                    resultModel.Msg = ex.Message;
                }
            }
            return resultModel;
        }


        public ResultModel BulkCopy(List<ORG_UNIT_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;

            //刪除資料表格
            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            //複製到SQL SERV
            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORG_ID", typeof(long));
            table.Columns.Add("ORG_NAME", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));

            foreach (var org in list)
            {
                var row = table.NewRow();
                row["ORG_ID"] = org.ORG_ID;
                row["ORG_NAME"] = org.ORG_NAME;
                row["CONTROL_FLAG"] = org.CONTROL_FLAG;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 5000;
                    bulkCopy.ColumnMappings.Add("ORG_ID", "ORG_ID");
                    bulkCopy.ColumnMappings.Add("ORG_NAME", "ORG_NAME");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.WriteToServer(table);
                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;
                }
            }

            if (!resultModel.Success)
                return resultModel;

            //call sp
            using (var cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.Transaction = transaction;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_OrgUnitSync";
                    var reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.HasRows)
                    {
                        resultModel.Code = reader.GetInt32(0);
                        resultModel.Msg = reader.GetString(1);
                        insertCount = reader.GetInt32(2);
                        updateCount = reader.GetInt32(3);
                        deleteCount = reader.GetInt32(4);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    resultModel.Code = -3;
                    resultModel.Msg = ex.Message;
                }
            }

            return resultModel;
        }


        public ResultModel BulkCopy(List<ORGANIZATION_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;

            //刪除資料表格
            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            //複製到SQL SERV
            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORGANIZATION_ID", typeof(long));
            table.Columns.Add("ORGANIZATION_CODE", typeof(string));
            table.Columns.Add("ORGANIZATION_NAME", typeof(string));
            table.Columns.Add("ORG_ID", typeof(long));
            table.Columns.Add("CONTROL_FLAG", typeof(string));

            foreach (var org in list)
            {
                var row = table.NewRow();
                row["ORGANIZATION_ID"] = org.ORGANIZATION_ID;
                row["ORGANIZATION_CODE"] = org.ORGANIZATION_CODE;
                row["ORGANIZATION_NAME"] = org.ORGANIZATION_NAME;
                row["ORG_ID"] = org.ORG_ID;
                row["CONTROL_FLAG"] = org.CONTROL_FLAG;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 5000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_CODE", "ORGANIZATION_CODE");
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_NAME", "ORGANIZATION_NAME");
                    bulkCopy.ColumnMappings.Add("ORG_ID", "ORG_ID");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.WriteToServer(table);
                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;
                }
            }

            if (!resultModel.Success)
                return resultModel;

            //call sp
            using(var cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.Transaction = transaction;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_OrganizaionSync";
                    var reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.HasRows)
                    {
                        resultModel.Code = reader.GetInt32(0);
                        resultModel.Msg = reader.GetString(1);
                        insertCount = reader.GetInt32(2);
                        updateCount = reader.GetInt32(3);
                        deleteCount = reader.GetInt32(4);
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    resultModel.Code = -3;
                    resultModel.Msg = ex.Message;
                }
            }

            return resultModel;
        }


        public ResultModel BulkCopy(List<SUBINVENTORY_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORGANIZATION_ID", typeof(long));
            table.Columns.Add("SUBINVENTORY_CODE", typeof(string));
            table.Columns.Add("SUBINVENTORY_NAME", typeof(string));
            table.Columns.Add("LOCATOR_TYPE", typeof(long));
            table.Columns.Add("OSP_FLAG", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));

            foreach (var sub in list)
            {
                var row = table.NewRow();
                row["ORGANIZATION_ID"] = sub.ORGANIZATION_ID;
                row["SUBINVENTORY_CODE"] = sub.SUBINVENTORY_CODE;
                row["SUBINVENTORY_NAME"] = sub.SUBINVENTORY_NAME;
                row["LOCATOR_TYPE"] = sub.LOCATOR_TYPE;
                row["OSP_FLAG"] = sub.OSP_FLAG;
                row["CONTROL_FLAG"] = sub.CONTROL_FLAG;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("SUBINVENTORY_CODE", "SUBINVENTORY_CODE");
                    bulkCopy.ColumnMappings.Add("SUBINVENTORY_NAME", "SUBINVENTORY_NAME");
                    bulkCopy.ColumnMappings.Add("LOCATOR_TYPE", "LOCATOR_TYPE");
                    bulkCopy.ColumnMappings.Add("OSP_FLAG", "OSP_FLAG");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;
                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp
            using (var cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.Transaction = transaction;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_SubinventorySync";
                    var reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.HasRows)
                    {
                        resultModel.Code = reader.GetInt32(0);
                        resultModel.Msg = reader.GetString(1);
                        insertCount = reader.GetInt32(2);
                        updateCount = reader.GetInt32(3);
                        deleteCount = reader.GetInt32(4);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    resultModel.Code = -3;
                    resultModel.Msg = ex.Message;
                }
            }


            return resultModel;
        }


        public ResultModel BulkCopy(List<LOCATOR_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();
            int insertCount = 0;
            int updateCount = 0;
            int deleteCount = 0;

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORGANIZATION_ID", typeof(long));
            table.Columns.Add("SUBINVENTORY_CODE", typeof(string));
            table.Columns.Add("LOCATOR_ID", typeof(long));
            table.Columns.Add("LOCATOR_SEGMENTS", typeof(string));
            table.Columns.Add("LOCATOR_DESC", typeof(string));
            table.Columns.Add("SEGMENT1", typeof(string));
            table.Columns.Add("SEGMENT2", typeof(string));
            table.Columns.Add("SEGMENT3", typeof(string));
            table.Columns.Add("SEGMENT4", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("LOCATOR_STATUS", typeof(long));
            table.Columns.Add("LOCATOR_STATUS_CODE", typeof(string));
            table.Columns.Add("LOCATOR_PICKING_ORDER", typeof(long));
            table.Columns.Add(new DataColumn("LOCATOR_DISABLE_DATE", typeof(DateTime)) { AllowDBNull= true });

            foreach (var locator in list)
            {
                var row = table.NewRow();
                row["ORGANIZATION_ID"] = locator.ORGANIZATION_ID;
                row["SUBINVENTORY_CODE"] = locator.SUBINVENTORY_CODE;
                row["LOCATOR_ID"] = locator.LOCATOR_ID;
                row["LOCATOR_SEGMENTS"] = locator.LOCATOR_SEGMENTS;
                row["LOCATOR_DESC"] = locator.LOCATOR_DESC;
                row["SEGMENT1"] = locator.SEGMENT1;
                row["SEGMENT2"] = locator.SEGMENT2;
                row["SEGMENT3"] = locator.SEGMENT3;
                row["SEGMENT4"] = locator.SEGMENT4;
                row["CONTROL_FLAG"] = locator.CONTROL_FLAG;
                row["LOCATOR_STATUS"] = locator.LOCATOR_STATUS;
                row["LOCATOR_STATUS_CODE"] = locator.LOCATOR_STATUS_CODE;
                row["LOCATOR_PICKING_ORDER"] = locator.LOCATOR_PICKING_ORDER;
                if(locator.LOCATOR_DISABLE_DATE.HasValue)
                {
                    row["LOCATOR_DISABLE_DATE"] = locator.LOCATOR_DISABLE_DATE.Value;
                }
                else
                {
                    row["LOCATOR_DISABLE_DATE"] = DBNull.Value;
                }
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("SUBINVENTORY_CODE", "SUBINVENTORY_CODE");
                    bulkCopy.ColumnMappings.Add("LOCATOR_ID", "LOCATOR_ID");
                    bulkCopy.ColumnMappings.Add("LOCATOR_SEGMENTS", "LOCATOR_SEGMENTS");
                    bulkCopy.ColumnMappings.Add("LOCATOR_DESC", "LOCATOR_DESC");
                    bulkCopy.ColumnMappings.Add("SEGMENT1", "SEGMENT1");
                    bulkCopy.ColumnMappings.Add("SEGMENT2", "SEGMENT2");
                    bulkCopy.ColumnMappings.Add("SEGMENT3", "SEGMENT3");
                    bulkCopy.ColumnMappings.Add("SEGMENT4", "SEGMENT4");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("LOCATOR_STATUS", "LOCATOR_STATUS");
                    bulkCopy.ColumnMappings.Add("LOCATOR_STATUS_CODE", "LOCATOR_STATUS_CODE");
                    bulkCopy.ColumnMappings.Add("LOCATOR_PICKING_ORDER", "LOCATOR_PICKING_ORDER");
                    bulkCopy.ColumnMappings.Add("LOCATOR_DISABLE_DATE", "LOCATOR_DISABLE_DATE");
                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp
            using (var cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.Transaction = transaction;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_LocatorSync";
                    var reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.HasRows)
                    {
                        resultModel.Code = reader.GetInt32(0);
                        resultModel.Msg = reader.GetString(1);
                        insertCount = reader.GetInt32(2);
                        updateCount = reader.GetInt32(3);
                        deleteCount = reader.GetInt32(4);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    resultModel.Code = -3;
                    resultModel.Msg = ex.Message;
                }
            }

            return resultModel;
        }


        public ResultModel BulkCopy(List<ITEMS_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("INVENTORY_ITEM_ID", typeof(long));
            table.Columns.Add("ITEM_NUMBER", typeof(string));
            table.Columns.Add("CATEGORY_CODE_INV", typeof(string));
            table.Columns.Add("CATEGORY_NAME_INV", typeof(string));
            table.Columns.Add("CATEGORY_CODE_COST", typeof(string));
            table.Columns.Add("CATEGORY_NAME_COST", typeof(string));
            table.Columns.Add("CATEGORY_CODE_CONTROL", typeof(string));
            table.Columns.Add("CATEGORY_NAME_CONTROL", typeof(string));
            table.Columns.Add("ITEM_DESC_ENG", typeof(string));
            table.Columns.Add("ITEM_DESC_SCH", typeof(string));
            table.Columns.Add("ITEM_DESC_TCH", typeof(string));
            table.Columns.Add("PRIMARY_UOM_CODE", typeof(string));
            table.Columns.Add("SECONDARY_UOM_CODE", typeof(string));
            table.Columns.Add("INVENTORY_ITEM_STATUS_CODE", typeof(string));
            table.Columns.Add("ITEM_TYPE", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_010", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_020", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_030", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_040", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_050", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_060", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_070", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_080", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_090", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_100", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_110", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_120", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_130", typeof(string));
            table.Columns.Add("CATALOG_ELEM_VAL_140", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("CREATED_BY", typeof(long));
            table.Columns.Add("CREATION_DATE", typeof(DateTime));
            table.Columns.Add("LAST_UPDATED_BY", typeof(long));
            table.Columns.Add("LAST_UPDATED_DATE", typeof(DateTime));

            foreach (var item in list)
            {
                var row = table.NewRow();
                row["INVENTORY_ITEM_ID"] = item.INVENTORY_ITEM_ID;
                row["ITEM_NUMBER"] = item.ITEM_NUMBER;
                row["CATEGORY_CODE_INV"] = item.CATEGORY_CODE_INV;
                row["CATEGORY_NAME_INV"] = item.CATEGORY_NAME_INV;
                row["CATEGORY_CODE_COST"] = item.CATEGORY_CODE_COST;
                row["CATEGORY_NAME_COST"] = item.CATEGORY_NAME_COST;
                row["CATEGORY_CODE_CONTROL"] = item.CATEGORY_CODE_CONTROL;
                row["CATEGORY_NAME_CONTROL"] = item.CATEGORY_NAME_CONTROL;
                row["ITEM_DESC_ENG"] = item.ITEM_DESC_ENG;
                row["ITEM_DESC_SCH"] = item.ITEM_DESC_SCH;
                row["ITEM_DESC_TCH"] = item.ITEM_DESC_TCH;
                row["PRIMARY_UOM_CODE"] = item.PRIMARY_UOM_CODE;
                row["SECONDARY_UOM_CODE"] = item.SECONDARY_UOM_CODE;
                row["INVENTORY_ITEM_STATUS_CODE"] = item.INVENTORY_ITEM_STATUS_CODE;
                row["ITEM_TYPE"] = item.ITEM_TYPE;
                row["CATALOG_ELEM_VAL_010"] = item.CATALOG_ELEM_VAL_010;
                row["CATALOG_ELEM_VAL_020"] = item.CATALOG_ELEM_VAL_020;
                row["CATALOG_ELEM_VAL_030"] = item.CATALOG_ELEM_VAL_030;
                row["CATALOG_ELEM_VAL_040"] = item.CATALOG_ELEM_VAL_040;
                row["CATALOG_ELEM_VAL_050"] = item.CATALOG_ELEM_VAL_050;
                row["CATALOG_ELEM_VAL_060"] = item.CATALOG_ELEM_VAL_060;
                row["CATALOG_ELEM_VAL_070"] = item.CATALOG_ELEM_VAL_070;
                row["CATALOG_ELEM_VAL_080"] = item.CATALOG_ELEM_VAL_080;
                row["CATALOG_ELEM_VAL_090"] = item.CATALOG_ELEM_VAL_090;
                row["CATALOG_ELEM_VAL_100"] = item.CATALOG_ELEM_VAL_100;
                row["CATALOG_ELEM_VAL_110"] = item.CATALOG_ELEM_VAL_110;
                row["CATALOG_ELEM_VAL_120"] = item.CATALOG_ELEM_VAL_120;
                row["CATALOG_ELEM_VAL_130"] = item.CATALOG_ELEM_VAL_130;
                row["CATALOG_ELEM_VAL_140"] = item.CATALOG_ELEM_VAL_140;
                row["CONTROL_FLAG"] = item.CONTROL_FLAG;
                row["CREATED_BY"] = item.CREATED_BY;
                row["CREATION_DATE"] = item.CREATION_DATE;
                row["LAST_UPDATED_BY"] = item.LAST_UPDATE_BY;
                row["LAST_UPDATED_DATE"] = item.LAST_UPDATE_DATE;

                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("INVENTORY_ITEM_ID", "INVENTORY_ITEM_ID");
                    bulkCopy.ColumnMappings.Add("ITEM_NUMBER", "ITEM_NUMBER");
                    bulkCopy.ColumnMappings.Add("CATEGORY_CODE_INV", "CATEGORY_CODE_INV");
                    bulkCopy.ColumnMappings.Add("CATEGORY_NAME_INV", "CATEGORY_NAME_INV");
                    bulkCopy.ColumnMappings.Add("CATEGORY_CODE_COST", "CATEGORY_CODE_COST");
                    bulkCopy.ColumnMappings.Add("CATEGORY_NAME_COST", "CATEGORY_NAME_COST");
                    bulkCopy.ColumnMappings.Add("CATEGORY_CODE_CONTROL", "CATEGORY_CODE_CONTROL");
                    bulkCopy.ColumnMappings.Add("CATEGORY_NAME_CONTROL", "CATEGORY_NAME_CONTROL");
                    bulkCopy.ColumnMappings.Add("ITEM_DESC_ENG", "ITEM_DESC_ENG");
                    bulkCopy.ColumnMappings.Add("ITEM_DESC_SCH", "ITEM_DESC_SCH");
                    bulkCopy.ColumnMappings.Add("ITEM_DESC_TCH", "ITEM_DESC_TCH");
                    bulkCopy.ColumnMappings.Add("PRIMARY_UOM_CODE", "PRIMARY_UOM_CODE");
                    bulkCopy.ColumnMappings.Add("SECONDARY_UOM_CODE", "SECONDARY_UOM_CODE");
                    bulkCopy.ColumnMappings.Add("INVENTORY_ITEM_STATUS_CODE", "INVENTORY_ITEM_STATUS_CODE");
                    bulkCopy.ColumnMappings.Add("ITEM_TYPE", "ITEM_TYPE");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_010", "CATALOG_ELEM_VAL_010");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_020", "CATALOG_ELEM_VAL_020");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_030", "CATALOG_ELEM_VAL_030");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_040", "CATALOG_ELEM_VAL_040");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_050", "CATALOG_ELEM_VAL_050");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_060", "CATALOG_ELEM_VAL_060");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_070", "CATALOG_ELEM_VAL_070");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_080", "CATALOG_ELEM_VAL_080");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_090", "CATALOG_ELEM_VAL_090");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_100", "CATALOG_ELEM_VAL_100");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_110", "CATALOG_ELEM_VAL_110");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_120", "CATALOG_ELEM_VAL_120");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_130", "CATALOG_ELEM_VAL_130");
                    bulkCopy.ColumnMappings.Add("CATALOG_ELEM_VAL_140", "CATALOG_ELEM_VAL_140");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("CREATED_BY", "CREATED_BY");
                    bulkCopy.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_BY", "LAST_UPDATED_BY");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_DATE", "LAST_UPDATED_DATE");

                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp

            return resultModel;
        }


        public ResultModel BulkCopy(List<TRANSACTION_TYPE_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("TRANSACTION_TYPE_ID", typeof(long));
            table.Columns.Add("TRANSACTION_TYPE_NAME", typeof(string));
            table.Columns.Add("DESCRIPTION", typeof(string));
            table.Columns.Add("TRANSACTION_ACTION_ID", typeof(long));
            table.Columns.Add("TRANSACTION_ACTION_NAME", typeof(string));
            table.Columns.Add("TRANSACTION_SOURCE_TYPE_ID", typeof(long));
            table.Columns.Add("TRANSACTION_SOURCE_TYPE_NAME", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("CREATED_BY", typeof(long));
            table.Columns.Add("CREATION_DATE", typeof(DateTime));
            table.Columns.Add("LAST_UPDATED_BY", typeof(long));
            table.Columns.Add("LAST_UPDATED_DATE", typeof(DateTime));

            foreach (var txn in list)
            {
                var row = table.NewRow();
                row["TRANSACTION_TYPE_ID"] = txn.TRANSACTION_TYPE_ID;
                row["TRANSACTION_TYPE_NAME"] = txn.TRANSACTION_TYPE_NAME;
                row["DESCRIPTION"] = txn.DESCRIPTION;
                row["TRANSACTION_ACTION_ID"] = txn.TRANSACTION_ACTION_ID;
                row["TRANSACTION_ACTION_NAME"] = txn.TRANSACTION_ACTION_NAME;
                row["TRANSACTION_SOURCE_TYPE_ID"] = txn.TRANSACTION_SOURCE_TYPE_ID;
                row["TRANSACTION_SOURCE_TYPE_NAME"] = txn.TRANSACTION_SOURCE_TYPE_NAME;
                row["CONTROL_FLAG"] = txn.CONTROL_FLAG;
                row["CREATED_BY"] = txn.CREATED_BY;
                row["CREATION_DATE"] = txn.CREATION_DATE;
                row["LAST_UPDATED_BY"] = txn.LAST_UPDATE_BY;
                row["LAST_UPDATED_DATE"] = txn.LAST_UPDATE_DATE;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("TRANSACTION_TYPE_ID", "TRANSACTION_TYPE_ID");
                    bulkCopy.ColumnMappings.Add("TRANSACTION_TYPE_NAME", "TRANSACTION_TYPE_NAME");
                    bulkCopy.ColumnMappings.Add("DESCRIPTION", "DESCRIPTION");
                    bulkCopy.ColumnMappings.Add("TRANSACTION_ACTION_ID", "TRANSACTION_ACTION_ID");
                    bulkCopy.ColumnMappings.Add("TRANSACTION_ACTION_NAME", "TRANSACTION_ACTION_NAME");
                    bulkCopy.ColumnMappings.Add("TRANSACTION_SOURCE_TYPE_ID", "TRANSACTION_SOURCE_TYPE_ID");
                    bulkCopy.ColumnMappings.Add("TRANSACTION_SOURCE_TYPE_NAME", "TRANSACTION_SOURCE_TYPE_NAME");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("CREATED_BY", "CREATED_BY");
                    bulkCopy.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_BY", "LAST_UPDATED_BY");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_DATE", "LAST_UPDATED_DATE");

                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp

            return resultModel;
        }


        public ResultModel BulkCopy(List<MACHINE_PAPER_TYPE_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORGANIZATION_ID", typeof(long));
            table.Columns.Add("ORGANIZATION_CODE", typeof(string));
            table.Columns.Add("MACHINE_CODE", typeof(string));
            table.Columns.Add("MACHINE_MEANING", typeof(string));
            table.Columns.Add("DESCRIPTION", typeof(string));
            table.Columns.Add("PAPER_TYPE", typeof(string));
            table.Columns.Add("MACHINE_NUM", typeof(string));
            table.Columns.Add("SUPPLIER_NUM", typeof(string));
            table.Columns.Add("SUPPLIER_NAME", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("CREATED_BY", typeof(long));
            table.Columns.Add("CREATION_DATE", typeof(DateTime));
            table.Columns.Add("LAST_UPDATED_BY", typeof(long));
            table.Columns.Add("LAST_UPDATED_DATE", typeof(DateTime));

            foreach (var type in list)
            {
                var row = table.NewRow();
                row["ORGANIZATION_ID"] = type.ORGANIZATION_ID;
                row["ORGANIZATION_CODE"] = type.ORGANIZATION_CODE;
                row["MACHINE_CODE"] = type.MACHINE_CODE;
                row["MACHINE_MEANING"] = type.MACHINE_MEANING;
                row["DESCRIPTION"] = type.DESCRIPTION;
                row["PAPER_TYPE"] = type.PAPER_TYPE;
                row["MACHINE_NUM"] = type.MACHINE_NUM;
                row["SUPPLIER_NUM"] = type.SUPPLIER_NUM;
                row["SUPPLIER_NAME"] = type.SUPPLIER_NAME;
                row["CONTROL_FLAG"] = type.CONTROL_FLAG;
                row["CREATED_BY"] = type.CREATED_BY;
                row["CREATION_DATE"] = type.CREATION_DATE;
                row["LAST_UPDATED_BY"] = type.LAST_UPDATE_BY;
                row["LAST_UPDATED_DATE"] = type.LAST_UPDATE_DATE;

                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_CODE", "ORGANIZATION_CODE");
                    bulkCopy.ColumnMappings.Add("MACHINE_CODE", "MACHINE_CODE");
                    bulkCopy.ColumnMappings.Add("MACHINE_MEANING", "MACHINE_MEANING");
                    bulkCopy.ColumnMappings.Add("DESCRIPTION", "DESCRIPTION");
                    bulkCopy.ColumnMappings.Add("PAPER_TYPE", "PAPER_TYPE");
                    bulkCopy.ColumnMappings.Add("MACHINE_NUM", "MACHINE_NUM");
                    bulkCopy.ColumnMappings.Add("SUPPLIER_NUM", "SUPPLIER_NUM");
                    bulkCopy.ColumnMappings.Add("SUPPLIER_NAME", "SUPPLIER_NAME");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("CREATED_BY", "CREATED_BY");
                    bulkCopy.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_BY", "LAST_UPDATED_BY");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_DATE", "LAST_UPDATED_DATE");

                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp

            return resultModel;
        }


        public ResultModel BulkCopy(List<RELATED_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("INVENTORY_ITEM_ID", typeof(long));
            table.Columns.Add("ITEM_NUMBER", typeof(string));
            table.Columns.Add("ITEM_DESCRIPTION", typeof(string));
            table.Columns.Add("RELATED_ITEM_ID", typeof(long));
            table.Columns.Add("RELATED_ITEM_NUMBER", typeof(string));
            table.Columns.Add("RELATED_ITEM_DESCRIPTION", typeof(string));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("CREATED_BY", typeof(long));
            table.Columns.Add("CREATION_DATE", typeof(DateTime));
            table.Columns.Add("LAST_UPDATED_BY", typeof(long));
            table.Columns.Add("LAST_UPDATED_DATE", typeof(DateTime));

            foreach (var related in list)
            {
                var row = table.NewRow();
                row["INVENTORY_ITEM_ID"] = related.INVENTORY_ITEM_ID;
                row["ITEM_NUMBER"] = related.ITEM_NUMBER;
                row["ITEM_DESCRIPTION"] = related.ITEM_DESCRIPTION;
                row["RELATED_ITEM_ID"] = related.RELATED_ITEM_ID;
                row["RELATED_ITEM_NUMBER"] = related.RELATED_ITEM_NUMBER;
                row["RELATED_ITEM_DESCRIPTION"] = related.RELATED_ITEM_DESCRIPTION;
                row["CONTROL_FLAG"] = related.CONTROL_FLAG;
                row["CREATED_BY"] = related.CREATED_BY;
                row["CREATION_DATE"] = related.CREATION_DATE;
                row["LAST_UPDATED_BY"] = related.LAST_UPDATE_BY;
                row["LAST_UPDATED_DATE"] = related.LAST_UPDATE_DATE;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_CODE", "ORGANIZATION_CODE");
                    bulkCopy.ColumnMappings.Add("MACHINE_CODE", "MACHINE_CODE");
                    bulkCopy.ColumnMappings.Add("MACHINE_MEANING", "MACHINE_MEANING");
                    bulkCopy.ColumnMappings.Add("DESCRIPTION", "DESCRIPTION");
                    bulkCopy.ColumnMappings.Add("PAPER_TYPE", "PAPER_TYPE");
                    bulkCopy.ColumnMappings.Add("MACHINE_NUM", "MACHINE_NUM");
                    bulkCopy.ColumnMappings.Add("SUPPLIER_NUM", "SUPPLIER_NUM");
                    bulkCopy.ColumnMappings.Add("SUPPLIER_NAME", "SUPPLIER_NAME");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("CREATED_BY", "CREATED_BY");
                    bulkCopy.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_BY", "LAST_UPDATED_BY");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_DATE", "LAST_UPDATED_DATE");

                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp

            return resultModel;
        }


        public ResultModel BulkCopy(List<YSZMPCKQ_TMP_T> list, string tableName, bool deleteTable = true)
        {
            var resultModel = new ResultModel();

            if (deleteTable)
            {
                resultModel = DeleteTable(tableName);

                if (!resultModel.Success) return resultModel;
            }

            DataTable table = new DataTable(tableName);
            table.Columns.Add("ORGANIZATION_ID", typeof(long));
            table.Columns.Add("ORGANIZATION_CODE", typeof(string));
            table.Columns.Add("OSP_SUBINVENTORY", typeof(string));
            table.Columns.Add("PSTYP", typeof(string));
            table.Columns.Add("BWETUP", typeof(decimal));
            table.Columns.Add("BWETDN", typeof(decimal));
            table.Columns.Add("RWTUP", typeof(decimal));
            table.Columns.Add("RWTDN", typeof(decimal));
            table.Columns.Add("PCKQ", typeof(long));
            table.Columns.Add("PAPER_QTY", typeof(long));
            table.Columns.Add("PIECES_QTY", typeof(long));
            table.Columns.Add("CONTROL_FLAG", typeof(string));
            table.Columns.Add("CREATED_BY", typeof(long));
            table.Columns.Add("CREATION_DATE", typeof(DateTime));
            table.Columns.Add("LAST_UPDATED_BY", typeof(long));
            table.Columns.Add("LAST_UPDATED_DATE", typeof(DateTime));

            foreach (var yszmpckq in list)
            {
                var row = table.NewRow();
                row["ORGANIZATION_ID"] = yszmpckq.ORGANIZATION_ID;
                row["ORGANIZATION_CODE"] = yszmpckq.ORGANIZATION_CODE;
                row["OSP_SUBINVENTORY"] = yszmpckq.OSP_SUBINVENTORY;
                row["PSTYP"] = yszmpckq.PSTYP;
                row["BWETUP"] = yszmpckq.BWETUP;
                row["BWETDN"] = yszmpckq.BWETDN;
                row["RWTUP"] = yszmpckq.RWTUP;
                row["RWTDN"] = yszmpckq.RWTDN;
                row["PCKQ"] = yszmpckq.PCKQ;
                row["PAPER_QTY"] = yszmpckq.PAPER_QTY;
                row["PIECES_QTY"] = yszmpckq.PIECES_QTY;
                row["CONTROL_FLAG"] = yszmpckq.CONTROL_FLAG;
                row["CREATED_BY"] = yszmpckq.CREATED_BY;
                row["CREATION_DATE"] = yszmpckq.CREATION_DATE;
                row["LAST_UPDATED_BY"] = yszmpckq.LAST_UPDATE_BY;
                row["LAST_UPDATED_DATE"] = yszmpckq.LAST_UPDATE_DATE;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_ID", "ORGANIZATION_ID");
                    bulkCopy.ColumnMappings.Add("ORGANIZATION_CODE", "ORGANIZATION_CODE");
                    bulkCopy.ColumnMappings.Add("OSP_SUBINVENTORY", "OSP_SUBINVENTORY");
                    bulkCopy.ColumnMappings.Add("PSTYP", "PSTYP");
                    bulkCopy.ColumnMappings.Add("BWETUP", "BWETUP");
                    bulkCopy.ColumnMappings.Add("BWETDN", "BWETDN");
                    bulkCopy.ColumnMappings.Add("RWTUP", "RWTUP");
                    bulkCopy.ColumnMappings.Add("RWTDN", "RWTDN");
                    bulkCopy.ColumnMappings.Add("PCKQ", "PCKQ");
                    bulkCopy.ColumnMappings.Add("PAPER_QTY", "PAPER_QTY");
                    bulkCopy.ColumnMappings.Add("PIECES_QTY", "PIECES_QTY");
                    bulkCopy.ColumnMappings.Add("CONTROL_FLAG", "CONTROL_FLAG");
                    bulkCopy.ColumnMappings.Add("CREATED_BY", "CREATED_BY");
                    bulkCopy.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_BY", "LAST_UPDATED_BY");
                    bulkCopy.ColumnMappings.Add("LAST_UPDATED_DATE", "LAST_UPDATED_DATE");

                    bulkCopy.WriteToServer(table);

                    resultModel.Success = true;
                    resultModel.Code = ResultModel.CODE_SUCCESS;
                    resultModel.Msg = "";
                }
                catch (Exception ex)
                {
                    resultModel.Code = -2;
                    resultModel.Success = false;
                    resultModel.Msg = ex.Message;

                }

            }

            if (!resultModel.Success)
                return resultModel;

            //call sp

            return resultModel;
        }
    }
}
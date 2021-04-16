using CHPOUTSRCMES.TASK.Models.Repository.Interface;
using CHPOUTSRCMES.TASK.Models.Repository.Oracle;
using CHPOUTSRCMES.TASK.Models.Views;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.UnitOfWork
{
    public class MasterUOW : UnitOfWork
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

#if BIOTECH
        private string schemaName = "TPMC_ADMIN";
        private string schemaNameOfUomConversion = "TPMC_ADMIN";
#else
        private string schemaName = "XXOSP";
        private string schemaNameOfUomConversion = "YFY_DIS_PKG_UTIL";
#endif

        public string SchemaName
        {
            set { schemaName = value;  }
            get { return schemaName + (schemaName.Length > 0 ? "." : ""); }
        }

        public string SchemaNameOfUomConversion
        {
            set { schemaNameOfUomConversion = value; }
            get { return schemaNameOfUomConversion + (schemaNameOfUomConversion.Length > 0 ? "." : ""); }
        }

        private IGenericRepository<XXCINV_SUBINVENTORY_V> subinventoryRepository = null;
        public IGenericRepository<XXCINV_SUBINVENTORY_V> SubinventoryRespository => subinventoryRepository ?? 
            (subinventoryRepository = new GenericRepository<XXCINV_SUBINVENTORY_V>(Context, $"{SchemaName}XXCINV_SUBINVENTORY_V"));

        private IGenericRepository<XXCINV_TRANSACTION_TYPE_V> transactionTypeRepository = null;
        public IGenericRepository<XXCINV_TRANSACTION_TYPE_V> TransactionTypeRepository => transactionTypeRepository ?? 
            (transactionTypeRepository = new GenericRepository<XXCINV_TRANSACTION_TYPE_V>(Context, $"{SchemaName}XXCINV_TRANSACTION_TYPE_V"));

        private ItemRepository itemRepository = null;
        public ItemRepository ItemRepository => itemRepository ?? 
            (itemRepository = new ItemRepository(Context, $"{SchemaName}XXCINV_MES_ITEMS_FTY_V"));

        private IGenericRepository<XXCOM_YSZMPCKQ_V> yszmpckqRepository = null;
        public IGenericRepository<XXCOM_YSZMPCKQ_V> YszmpckqRepository => yszmpckqRepository ?? 
            (yszmpckqRepository = new GenericRepository<XXCOM_YSZMPCKQ_V>(Context, $"{SchemaName}XXCOM_YSZMPCKQ_V"));

        private IGenericRepository<XXCPO_MACHINE_PAPER_TYPE_V> machinePaperTypeRepository = null;
        public IGenericRepository<XXCPO_MACHINE_PAPER_TYPE_V> MachinePaperTypeRepository => machinePaperTypeRepository ?? 
            (machinePaperTypeRepository = new GenericRepository<XXCPO_MACHINE_PAPER_TYPE_V>(Context, $"{SchemaName}XXCPO_MACHINE_PAPER_TYPE_V"));

        private IGenericRepository<XXCINV_OSP_RELATED_ITEM_V> ospRelatedItemRepository = null;
        public IGenericRepository<XXCINV_OSP_RELATED_ITEM_V> OspRelatedItemRepository => ospRelatedItemRepository ?? 
            (ospRelatedItemRepository = new GenericRepository<XXCINV_OSP_RELATED_ITEM_V>(Context, $"{SchemaName}XXCINV_OSP_RELATED_ITEM_V"));


        public MasterUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);
        }

        public async Task<IEnumerable<XXCINV_SUBINVENTORY_V>> GetSubinventoryListAsync()
        {
            var list = await SubinventoryRespository.GetAllAsync();
            return list;
        }

        public async Task<IEnumerable<XXCINV_TRANSACTION_TYPE_V>> GetTransactionTypeListAsync()
        {
            var list = await TransactionTypeRepository.GetAllAsync();
            return list;
        }

        public async Task<IEnumerable<XXCINV_MES_ITEMS_FTY_V>> GetItemRangeAsync(long offset, long count)
        {
            var list = await ItemRepository.GetRangeAsync(offset, count);
            return list;
        }

        public async Task<IEnumerable<XXCINV_MES_ITEMS_FTY_V>> GetItemListAsync()
        {
            var list = await ItemRepository.GetAllAsync();
            return list;
        }

        public async Task<long> GetItemCountAsync()
        {
            return await ItemRepository.CountAsync();
        }

        public async Task<DateTime> GetItemLastUpdateDateAsync()
        {
            return await ItemRepository.GetLastUpdateDateAsync();
        }

        public async Task<IEnumerable<XXCINV_MES_ITEMS_FTY_V>> GetAllItemByLastUpdateDate(DateTime date)
        {
            return await ItemRepository.GetAllByLastUpdateDate(date);
        }

        public async Task<long> ItemCountByLastUpdateDateAsync(DateTime date)
        {
            return await ItemRepository.CountByLastUpdateDateAsync(date);
        }

        public async Task<IEnumerable<XXCOM_YSZMPCKQ_V>> GetYszmpckqListAsync()
        {
            var list = await YszmpckqRepository.GetAllAsync();
            return list;
        }

        public async Task<IEnumerable<XXCPO_MACHINE_PAPER_TYPE_V>> GetMachinePaperTypeListAsync()
        {
            var list = await MachinePaperTypeRepository.GetAllAsync();
            return list;
        }

        public async Task<IEnumerable<XXCINV_OSP_RELATED_ITEM_V>> GetOspRelatedItemListAsync()
        {
            var list = await OspRelatedItemRepository.GetAllAsync();
            return list;
        }


        public async Task<decimal?> UomConvertAsync(long itemId, decimal amount, string fromUom, string toUom, int round = 5) 
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(":ITEM_ID", itemId);
                parameters.Add(":FROM_QTY", amount);
                parameters.Add(":FROM_UOM", fromUom);
                parameters.Add(":TO_UOM", toUom);
                parameters.Add(":ROUND", round);

                return await Context.QueryFirstAsync<decimal>(
$"SELECT ROUND({SchemaNameOfUomConversion}UOM_CONVERSION(:ITEM_ID, :FROM_QTY, :FROM_UOM, :TO_UOM), :ROUND) QUANTITY FROM DUAL", parameters
                    );
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            

            return null;
        }

        public decimal? UomConvert(long itemId, decimal amount, string fromUom, string toUom, int round = 5)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(":ITEM_ID", itemId);
                parameters.Add(":FROM_QTY", amount);
                parameters.Add(":FROM_UOM", fromUom);
                parameters.Add(":TO_UOM", toUom);
                parameters.Add(":ROUND", round);

                return Context.QueryFirst<decimal>($"SELECT ROUND({SchemaNameOfUomConversion}UOM_CONVERSION(:ITEM_ID, :FROM_QTY, :FROM_UOM, :TO_UOM), :ROUND) QUANTITY FROM DUAL", parameters
                    );
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return null;
        }

        public XXCINV_MES_ITEMS_FTY_V GetItemAsync(string itemNo)
        {
            try
            {
                DynamicParameters itemQueryParams = new DynamicParameters();
                itemQueryParams.Add(":ITEM_NO", itemNo, DbType.String, ParameterDirection.Input, 40);
                string cmd =
$@"SELECT INVENTORY_ITEM_ID
,ITEM_NUMBER
,CATEGORY_CODE_INV
,CATEGORY_NAME_INV
,CATEGORY_CODE_COST
,CATEGORY_NAME_COST
,CATEGORY_CODE_CONTROL
,CATEGORY_NAME_CONTROL
,ITEM_DESC_ENG
,ITEM_DESC_SCH
,ITEM_DESC_TCH
,PRIMARY_UOM_CODE
,SECONDARY_UOM_CODE
,INVENTORY_ITEM_STATUS_CODE
,ITEM_TYPE
,CATALOG_ELEM_VAL_010
,CATALOG_ELEM_VAL_020
,CATALOG_ELEM_VAL_030
,CATALOG_ELEM_VAL_040
,CATALOG_ELEM_VAL_050
,CATALOG_ELEM_VAL_060
,CATALOG_ELEM_VAL_070
,CATALOG_ELEM_VAL_080
,CATALOG_ELEM_VAL_090
,CATALOG_ELEM_VAL_100
,CATALOG_ELEM_VAL_110
,CATALOG_ELEM_VAL_120
,CATALOG_ELEM_VAL_130
,CATALOG_ELEM_VAL_140
,CREATED_BY
,CREATION_DATE
,LAST_UPDATED_BY
,LAST_UPDATE_DATE FROM {SchemaName}XXCINV_MES_ITEMS_FTY_V WHERE ITEM_NUMBER = :ITEM_NO";


                 return Context.Query<XXCINV_MES_ITEMS_FTY_V>(cmd, itemQueryParams).SingleOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return null;
        }
    }
}

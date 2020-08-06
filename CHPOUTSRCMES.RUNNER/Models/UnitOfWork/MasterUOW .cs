using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Linq;
using CHPOUTSRCMES.TASK.Models.Repository;
using CHPOUTSRCMES.TASK.Models.Repository.Interface;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using CHPOUTSRCMES.TASK.Models.Views;
using Dapper;

namespace BatchPrint.Model.UnitOfWork
{
    public class MasterUOW : UnitOfWork
    {
#if DEBUG
        private string schemaName = "TPMC_ADMIN";
        private string schemaNameOfUomConversion = "TPMC_ADMIN";
#else
        private string schemaName = "XXOSP";
        private string schemaNameOfUomConversion = "YFY_DIS_PKG_UTIL";
#endif

        public string SchemaName
        {
            set { schemaName = value;  }
            get { return SchemaName + (SchemaName.Length > 0 ? "." : ""); }
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

        private IGenericRepository<XXIFV050_ITEMS_FTY_V> itemRepository = null;
        public IGenericRepository<XXIFV050_ITEMS_FTY_V> ItemRepository => itemRepository ?? 
            (itemRepository = new GenericRepository<XXIFV050_ITEMS_FTY_V>(Context, $"{SchemaName}XXIFV050_ITEMS_FTY_V"));

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

        public async Task<IEnumerable<XXIFV050_ITEMS_FTY_V>> GetItemListAsync()
        {
            var list = await ItemRepository.GetAllAsync();
            return list;
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


        public async Task<decimal> UomConvertAsync(long itemId, decimal amount, string fromUom, string toUom, int round = 5) 
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":ITEM_ID", itemId);
            parameters.Add(":FROM_QTY", amount);
            parameters.Add(":FROM_UOM", fromUom);
            parameters.Add(":TO_UOM", toUom);
            parameters.Add(":ROUND", round);


            var data = await Context.QueryFirstAsync<decimal>(
                $"SELECT ROUND({SchemaNameOfUomConversion}UOM_CONVERSION(:ITEM_ID, :FROM_QTY, :FROM_UOM, :TO_UOM), :ROUND) QUANTITY FROM DUAL", parameters
                );

            return data;
        }

        public XXIFV050_ITEMS_FTY_V GetItemAsync(string itemNo)
        {
            
            DynamicParameters itemQueryParams = new DynamicParameters();
            itemQueryParams.Add(":ITEM_NO", itemNo, DbType.String, ParameterDirection.Input, 40);
            var item = Context.Query<XXIFV050_ITEMS_FTY_V>($"SELECT * FROM XXOSP.XXIFV050_ITEMS_FTY_V WHERE ITEM_NUMBER = :ITEM_NO", itemQueryParams).SingleOrDefault();

            return item;
        }
    }
}

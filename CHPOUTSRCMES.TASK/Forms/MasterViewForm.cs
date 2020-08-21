using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using CHPOUTSRCMES.TASK.Models.Views;
using Dapper;
using NLog.Internal;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK.Forms
{
    public partial class MasterViewForm : ChildForm
    {
        BindingSource bsSubinventoryList;
        BindingSource bsTransactionTypeList;
        BindingSource bsMachinePaperTypeList;
        BindingSource bsItemList;
        BindingSource bsYszmpckqList;
        BindingSource bsOspRelatedItemList;

        List<XXCINV_SUBINVENTORY_V> subinventoryList { set; get; }
        List<XXCINV_TRANSACTION_TYPE_V> transactionTypeList { set; get; }
        List<XXCPO_MACHINE_PAPER_TYPE_V> machinePaperTypeList { set; get; }
        List<XXCOM_YSZMPCKQ_V> yszmpckqList { set; get; }
        List<XXCINV_MES_ITEMS_FTY_V> itemList { set; get; }
        List<XXCINV_OSP_RELATED_ITEM_V> ospRelatedItemList { set; get; }


        public MasterViewForm()
        {
            InitializeComponent();

            bn_Item.BindingSource = bsItemList;
        }

        private async void getSubinventoryList(MasterUOW uow)
        {
            subinventoryList = (await uow.GetSubinventoryListAsync()).ToList();
            bindDataGridView(subinventoryList, dgv_SubinventoryList, bsSubinventoryList);
        }

        private async void getMachinePaperTypeList(MasterUOW uow)
        {
            machinePaperTypeList = (await uow.GetMachinePaperTypeListAsync()).ToList();
            bindDataGridView(machinePaperTypeList, dgv_MachinePaperType, bsMachinePaperTypeList);
        }

        private async void getTransactionTypeList(MasterUOW uow)
        {
            transactionTypeList = (await uow.GetTransactionTypeListAsync()).ToList();
            bindDataGridView(transactionTypeList, dgv_TransactionType, bsTransactionTypeList);
        }

        private async void getItemList(MasterUOW uow)
        {
            itemList = (await uow.GetItemListAsync()).ToList();

            bindDataGridView(itemList, dgv_Item, bsItemList);
        }

        private async void getOspRelatedItemList(MasterUOW uow)
        {
            ospRelatedItemList = (await uow.GetOspRelatedItemListAsync()).ToList();
            bindDataGridView(ospRelatedItemList, dgv_OspRelatedItem, bsOspRelatedItemList);
        }
        private async void getYszmpckqList(MasterUOW uow)
        {
            yszmpckqList = (await uow.GetYszmpckqListAsync()).ToList();
            bindDataGridView(yszmpckqList, dgv_Yszmpckq, bsYszmpckqList);
        }

        private void tsb_Load_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (var conn = new OracleConnection(Controller.ErpConnStr))
                {
                    using (MasterUOW uow = new MasterUOW(conn, false))
                    {
                        //SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);
                        getSubinventoryList(uow);
                        getTransactionTypeList(uow);
                        getMachinePaperTypeList(uow);
                        getItemList(uow);
                        getOspRelatedItemList(uow);
                        getYszmpckqList(uow);
                    }
                }
            });
        }

        /// <summary>
        /// Binding DataGridView
        /// </summary>
        /// <param name="processList"></param>
        internal void bindDataGridView<T>(IEnumerable<T> list, DataGridView view, BindingSource source)
        {
            if (view.InvokeRequired)
            {
                view.Invoke(new EventHandler(delegate
                {
                    bindDataGridView(list, view, source);
                }));
                return;
            }

            try
            {

                if (source == null)
                {
                    source = new BindingSource();
                    source.DataSource = list;
                    view.DataSource = source;
                    view.Invalidate();
                }
                else
                {
                    source.DataSource = list;
                    source.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        
    }
}

using BatchPrint.Model.UnitOfWork;
using CHPOUTSRCMES.TASK.Models.Views;
using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK.Forms
{
    public partial class UomConverterForm : ChildForm
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public UomConverterForm()
        {
            InitializeComponent();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {

                errorProvider1.Clear();

                if (txt_Amount.Text.Trim().Length == 0)
                {
                    errorProvider1.SetError(txt_Amount, "數量必須輸入!");
                    //數量未輸入
                    return;
                }

                var amount = ToDecimal(txt_Amount.Text);

                if (amount == 0m)
                {
                    errorProvider1.SetError(txt_Amount, "數量必須輸入數字且大於0!");
                    //數量輸入須為數字且大於0
                    return;
                }


                if (txt_Uom.Text.Trim().Length == 0)
                {
                    errorProvider1.SetError(txt_Uom, "單位必須輸入!");
                    //單位未輸入
                    return;
                }

                if (txt_ToUom.Text.Trim().Length == 0)
                {
                    //換算單位未輸入
                    errorProvider1.SetError(txt_ToUom, "換算單位必須輸入!");
                    return;
                }

                if (txt_Item.Text.Trim().Length == 0)
                {
                    //料號未輸入
                    errorProvider1.SetError(txt_Item, "料號必須輸入!");
                    return;
                }
                string itemNo = txt_Item.Text.Trim();
                string fromUom = txt_Uom.Text.Trim();
                string toUom = txt_ToUom.Text.Trim();
                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();
                logger.Info($"Connection String:{cnstr}");
                using (var cn = new OracleConnection(cnstr))
                {
                    try
                    {
                        

                        using (MasterUOW masterUOW = new MasterUOW(cn))
                        {
                            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);

                            //料號檢查
                            //var item = masterUOW.Context.Query<XXIFV050_ITEMS_FTY_V>($"SELECT * FROM XXOSP.XXIFV050_ITEMS_FTY_V WHERE ITEM_NUMBER = '{txt_Item.Text}'").SingleOrDefault();
                            var item = masterUOW.GetItemAsync(itemNo);

                            if (item == null || item.INVENTORY_ITEM_ID == 0)
                            {
                                //料號不存在
                                errorProvider1.SetError(txt_Item, "料號不存在!");
                                return;
                            }

                            var data = await masterUOW.UomConvertAsync(item.INVENTORY_ITEM_ID, amount, fromUom, toUom, 5);
                            lbl_RestultAmount.Text = data.ToString();
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }
            catch
            {

            }

        }

        private decimal ToDecimal(string amount, decimal defaultValue = 0m)
        {
            decimal result = defaultValue;
            try
            {
                result = Convert.ToDecimal(amount);
            }
            finally
            {

            }

            return result;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();
            using (OracleConnection conn = new OracleConnection(cnstr))
            {
                using (MasterUOW uow = new MasterUOW(conn))
                {
                    SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);
                    var data = await uow.UomConvertAsync(559389, 100, "KG", "MT", 5);
                    MessageBox.Show(data.ToString());

                    Thread.Sleep(1000);

                    var item = uow.Context.Query<XXIFV050_ITEMS_FTY_V>(@"SELECT * FROM XXOSP.XXIFV050_ITEMS_FTY_V WHERE ITEM_NUMBER = '4AK0XA010001118RL00'").SingleOrDefault();


//                    var item = uow.Context.Query<XXIFV050_ITEMS_FTY_V>(@"
//SELECT INVENTORY_ITEM_ID
//      ,ITEM_NUMBER
//      ,CATEGORY_CODE_INV
//      ,CATEGORY_NAME_INV
//      ,CATEGORY_CODE_COST
//      ,CATEGORY_NAME_COST
//      ,CATEGORY_CODE_CONTROL
//      ,CATEGORY_NAME_CONTROL
//      ,ITEM_DESC_ENG
//      ,ITEM_DESC_SCH
//      ,ITEM_DESC_TCH
//      ,PRIMARY_UOM_CODE
//      ,SECONDARY_UOM_CODE
//      ,INVENTORY_ITEM_STATUS_CODE
//      ,ITEM_TYPE
//      ,CATALOG_ELEM_VAL_010
//      ,CATALOG_ELEM_VAL_020
//      ,CATALOG_ELEM_VAL_030
//      ,CATALOG_ELEM_VAL_040
//      ,CATALOG_ELEM_VAL_050
//      ,CATALOG_ELEM_VAL_060
//      ,CATALOG_ELEM_VAL_070
//      ,CATALOG_ELEM_VAL_080
//      ,CATALOG_ELEM_VAL_090
//      ,CATALOG_ELEM_VAL_100
//      ,CATALOG_ELEM_VAL_110
//      ,CATALOG_ELEM_VAL_120
//      ,CATALOG_ELEM_VAL_130
//      ,CATALOG_ELEM_VAL_140
//      ,CREATED_BY
//      ,CREATION_DATE
//      ,LAST_UPDATED_BY
//      ,LAST_UPDATE_DATE
//      FROM XXOSP.XXIFV050_ITEMS_FTY_V  WHERE ITEM_NUMBER ='4AK0XA010001118RL00'").SingleOrDefault();
                    MessageBox.Show(item.ToString());
                }

            }
        }
    }
}

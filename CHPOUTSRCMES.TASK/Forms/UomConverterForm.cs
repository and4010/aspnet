using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
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
                
                using (var cn = new OracleConnection(Controller.ErpConnStr))
                {
                    try
                    {
                        using (MasterUOW masterUOW = new MasterUOW(cn))
                        {
                            //SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);

                            //料號檢查
                            
                            var item = masterUOW.GetItemAsync(itemNo);

                            if (item == null || item.INVENTORY_ITEM_ID == 0)
                            {
                                //料號不存在
                                errorProvider1.SetError(txt_Item, "料號不存在!");
                                return;
                            }

                            var data = await masterUOW.UomConvertAsync(item.INVENTORY_ITEM_ID, amount, fromUom, toUom, 5);
                            lbl_RestultAmount.Text = (data != null) ? data.ToString() : "失敗";
                            
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

    }
}

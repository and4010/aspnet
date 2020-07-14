using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class CHP_PROCESS_T
    {
        [Display(Name = "加工表頭ID")] //OSP_BATCH_ID
        public int Process_Heard_Id { set; get; }

        [Display(Name = "加工明細ID")] //OSP_BATCH_ID
        public int Process_Detail_Id { set; get; }

        [Display(Name = "加工狀態")] //OSP_BATCH_STATUS_DESC
        public string Process_Status { set; get; }

        [Display(Name = "工單號")] //OSP_BATCH_NO
        public string Process_Batch_no { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "需求日期")] //DUE_DATE
        public DateTime Demand_Date { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "裁切日期(起)")] //PLAN_START_DATE
        public DateTime? Cutting_Date_From { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "裁切日期(迄)")] //PLAN_CMPLT_DATE
        public DateTime? Cutting_Date_To { set; get; }

        [Display(Name = "機台")]
        public string Manchine_Num { set; get; }

        [Display(Name = "客戶名稱")] //CUSTOMER_NAME
        public string Cosutomer_Num { set; get; }

        [Display(Name = "訂單編號")] //ORDER_NUMBER
        public string Order_Number { set; get; }

        [Display(Name = "明細行")] //ORDER_LINE_NUMBER
        public string Detail_Line { set; get; }

        [Display(Name = "基重")] //BASIC_WEIGHT
        public string Basic_Weight { set; get; }

        [Display(Name = "規格")] //SPECIFICATION
        public string Specification { set; get; }

        [Display(Name = "絲向")] //GRAIN_DIRECTION
        public string Grain_Direction { set; get; }

        [Display(Name = "令重")] //ORDER_WEIGHT
        public string Ream_Weight { set; get; }

        [Display(Name = "令數")] //REAM_WT
        public string Ream_Qty { set; get; }

        [Display(Name = "重量")] //PRIMARY_QUANTITY
        public string Weight { set; get; }

        [Display(Name = "主要單位")] //PRIMARY_UOM
        public string PrimaryUom { set; get; }

        [Display(Name = "交易單位")] //TRANSACTION_UOM
        public string TransactionUom { set; get; }

        [Display(Name = "包裝方式")]
        public string Packing_Type { set; get; }

        [Display(Name = "紙別")]
        public string Paper_Type { get; set; }

        [Display(Name = "委外工單備註")] //OSP_REMARK
        public string Outsourching_Remark {set;get;}

        [Display(Name = "生產料號")]
        public string Produce_Item { set; get; }

        [Display(Name = "組成成分料號")] //LINE_TYPE (I)
        public string SelectedInventoryItemNumber { set; get; }

        [Display(Name = "產品料號")]  //LINE_TYPE  (P)
        public string Product_Item { set; get; }

        [Display(Name = "生產備註")]
        public string Produce_Remark { set; get; }

        [Display(Name = "倉庫")] //SUBINVENTORY
        public string Subinventory { set; get; }

        [Display(Name = "儲位")] //LOCATOR_CODE
        public string Locator { get; set; }

        [Display(Name = "損耗量")]
        public string Loss { set; get; }

        [Display(Name = "建立人員ID")] //WIP_CREATED_BY
        public string Created_by { set; get; }

        [Display(Name = "建立日期")] //WIP_CREATION_DATE
        public string Creation_date { set; get; }

        [Display(Name = "最後更新人員ID")] //WIP_LAST_UPDATED_BY
        public string Last_updated_by { set; get; }

        [Display(Name = "最後更新日期")] //WIP_LAST_UPDATE_DATE
        public string Last_update_date { set; get; }




    }
}
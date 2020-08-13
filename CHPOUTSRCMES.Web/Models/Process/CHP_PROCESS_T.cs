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
        public long OspHeaderId { set; get; }

        [Display(Name = "加工明細ID")] //OSP_BATCH_ID
        public long OspDetailInId { set; get; }

        [Display(Name = "加工狀態")] //OSP_BATCH_STATUS_DESC
        public string Status { set; get; }

        [Display(Name = "工單號")] //OSP_BATCH_NO
        public string BatchNo { set; get; }

        [Display(Name = "工單類別")]
        public string BatchType { set;get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "需求日期")] //DUE_DATE
        public DateTime DueDate { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "裁切日期(起)")] //CUTTING_DATE_FROM
        public DateTime? CuttingDateFrom { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "裁切日期(迄)")] //CUTTING_DATE_TO
        public DateTime? CuttingDateTo { set; get; }

        [Display(Name = "機台")]
        public string MachineNum { set; get; }

        [Display(Name = "客戶名稱")] //CUSTOMER_NAME
        public string CustomerName { set; get; }

        [Display(Name = "訂單編號")] //ORDER_NUMBER
        public string OrderNumber { set; get; }

        [Display(Name = "明細行")] //ORDER_LINE_NUMBER
        public string OrderLineNumber { set; get; }

        [Display(Name = "基重")] //BASIC_WEIGHT
        public string BasicWeight { set; get; }

        [Display(Name = "規格")] //SPECIFICATION
        public string Specification { set; get; }

        [Display(Name = "絲向")] //GRAIN_DIRECTION
        public string GrainDirection { set; get; }

        [Display(Name = "令重")] //ORDER_WEIGHT
        public string OrderWeight { set; get; }

        [Display(Name = "令數")] //REAM_WT
        public string ReamWt { set; get; }

        [Display(Name = "主要重量")] //PRIMARY_QUANTITY
        public string PrimaryQuantity { set; get; }

        [Display(Name = "主要單位")] //PRIMARY_UOM
        public string PrimaryUom { set; get; }

        [Display(Name = "交易單位")] //TRANSACTION_UOM
        public string TransactionUom { set; get; }

        [Display(Name = "包裝方式")]
        public string PackingType { set; get; }

        [Display(Name = "紙別")]
        public string PaperType { get; set; }

        [Display(Name = "委外工單備註")] //OSP_REMARK
        public string OspRemark { set;get;}

        [Display(Name = "組成成分料號")] //LINE_TYPE (I)
        public string SelectedInventoryItemNumber { set; get; }

        [Display(Name = "產品料號")]  //LINE_TYPE  (P)
        public string Product_Item { set; get; }

        [Display(Name = "生產備註")]
        public string Note { set; get; }

        [Display(Name = "倉庫")] //SUBINVENTORY
        public string Subinventory { set; get; }

        [Display(Name = "儲位")] //LOCATOR_CODE
        public string LocatorCode { get; set; }

        [Display(Name = "損耗量")]
        public string Loss { set; get; }

        [Display(Name = "建立人員ID")] //WIP_CREATED_BY
        public string Createdby { set; get; }

        [Display(Name = "建立日期")] //WIP_CREATION_DATE
        public DateTime Creationdate { set; get; }

        [Display(Name = "最後更新人員ID")] //WIP_LAST_UPDATED_BY
        public string LastUpdatedBy { set; get; }

        [Display(Name = "最後更新日期")] //WIP_LAST_UPDATE_DATE
        public DateTime? LastUpdateDate { set; get; }




    }
}
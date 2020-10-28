using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Models.Process
{
    public class CHP_PROCESS_T
    {
        [Display(Name = "加工表頭ID")] 
        public long OspHeaderId { set; get; }

        [Display(Name = "加工明細ID")] 
        public long OspDetailInId { set; get; }

        [Display(Name = "加工產出明細ID")]
        public long OspDetailOutId { set; get; }

        [Display(Name = "加工狀態")] //STATUS
        public string Status { set; get; }

        [Display(Name = "工單號")] //BATCH_NO
        public string BatchNo { set; get; }

        /// <summary>
        /// 計劃開工日期
        /// </summary>
        [Display(Name = "計劃開工日期")] //PLAN_START_DATE
        public DateTime PlanStartDate { set; get; }

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

        [Display(Name = "客戶名稱")] 
        public string CustomerName { set; get; }

        [Display(Name = "訂單編號")] 
        public long OrderNumber { set; get; }

        [Display(Name = "明細行")] 
        public string OrderLineNumber { set; get; }

        [Display(Name = "基重")] 
        public string BasicWeight { set; get; }

        [Display(Name = "規格")] 
        public string Specification { set; get; }

        [Display(Name = "絲向")] 
        public string GrainDirection { set; get; }

        [Display(Name = "令重")] 
        public string OrderWeight { set; get; }

        [Display(Name = "令數")] 
        public string ReamWt { set; get; }

        [Display(Name = "主要重量")] //PRIMARY_QUANTITY
        public decimal PrimaryQuantity { set; get; }

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
        public decimal? Loss { set; get; }

        [Display(Name = "建立人員ID")] //CREATED_BY
        public string Createdby { set; get; }

        [Display(Name = "建立日期")] //CREATION_DATE
        public DateTime Creationdate { set; get; }

        [Display(Name = "最後更新人員ID")] //LAST_UPDATED_BY
        public string LastUpdatedBy { set; get; }

        [Display(Name = "最後更新日期")] //LAST_UPDATE_DATE
        public DateTime? LastUpdateDate { set; get; }

        [Display(Name = "紙別")]
        public string DoPaperType { get; set; }

        [Display(Name = "規格")]
        public string DoSpecification { set; get; }

        [Display(Name = "基重")]
        public string DoBasicWeight { set; get; }

        [Display(Name = "絲向")]
        public string DoGrainDirection { set; get; }

        [Display(Name = "令數")]
        public string DoReamWt { set; get; }

        [Display(Name = "主要重量")] //PRIMARY_QUANTITY
        public decimal DoPrimaryQuantity { set; get; }

        [Display(Name = "包裝方式")]
        public string DoPackingType { set; get; }

        public long? SrcOspHeaderId { set; get; }

        public string SrcBatchNo { set; get; }

        public long? OrgOspHeaderId { set; get; }
    }
}
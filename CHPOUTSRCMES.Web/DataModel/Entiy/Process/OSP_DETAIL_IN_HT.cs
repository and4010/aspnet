using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Process
{
    public class OSP_DETAIL_IN_HT
    {
        /// <summary>
        /// 加工明細ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_DETAIL_IN_HT_ID")]
        public long OspDetailInHtId { set; get; }

        /// <summary>
        /// 加工明細ID
        /// </summary>
        /// 
        [Required]
        [Column("OSP_DETAIL_IN_ID")]
        public long OspDetailInId { set; get; }

        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        [Required]
        [Column("OSP_HEADER_ID")]
        public long OspHeaderId { set; get; }

        /// <summary>
        /// 類別(I(-1)：組成成份、P(1)：產品)
        /// </summary>
        /// 
        [StringLength(01)]
        [Column("LINE_TYPE")]
        [Required]
        public string LineType { set; get; }

        /// <summary>
        /// 行號
        /// </summary>
        /// 
        [Column("LINE_NO")]
        [Required]
        public long LineNo { set; get; }


        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        [Required]
        [Column("INVENTORY_ITEM_ID")]
        public long InventoryItemId { set; get; }


        /// <summary>
        /// 料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("INVENTORY_ITEM_NUMBER")]
        public string InventoryItemNumber { set; get; }


        /// <summary>
        /// 基重
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("BASIC_WEIGHT")]
        public string BasicWeight { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("SPECIFICATION")]
        public string Specification { set; get; }

        /// <summary>
        /// 絲向
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("GRAIN_DIRECTION")]
        public string GrainDirection { set; get; }

        /// <summary>
        ///令重(產品)
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("ORDER_WEIGHT")]
        public string OrderWeight { set; get; }

        /// <summary>
        ///令數(產品)
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("REAM_WT")]
        public string ReamWt { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(4)]
        [Column("PAPER_TYPE")]
        public string PaperType { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }


        /// <summary>
        /// 計畫數量
        /// </summary>
        /// 
        [Required]
        [Column("PLAN_QTY")]
        public decimal PlanQty { set; get; }

        /// <summary>
        /// 在製品計畫數量
        /// </summary>
        /// 
        [Required]
        [Column("WIP_PLAN_QTY")]
        public decimal WipPLAN_QTY { set; get; }


        /// <summary>
        /// 單位
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("DTL_UOM")]
        public string DtlUom { set; get; }

        /// <summary>
        /// 訂單ID(產品)
        /// </summary>
        /// 
        [Column("ORDER_HEADER_ID")]
        public long OrderHeaderId { set; get; }


        /// <summary>
        ///訂單編號(產品)
        /// </summary>
        /// 
        [Column("ORDER_NUMBER")]
        public long OrderNumber { set; get; }

        /// <summary>
        ///訂單明細ID(產品)
        /// </summary>
        /// 
        [Column("ORDER_LINE_ID")]
        public long OrderLineId { set; get; }

        /// <summary>
        ///訂單明細行號(產品)
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("ORDER_LINE_NUMBER")]
        public string OrderLineNumber { set; get; }



        /// <summary>
        ///客戶ID(產品)
        /// </summary>
        /// 
        [Column("CUSTOMER_ID")]
        public long CustomerId { set; get; }

        /// <summary>
        ///客戶編號(產品)
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("CUSTOMER_NUMBER")]
        public string CustomerNumber { set; get; }

        /// <summary>
        ///客戶名稱(產品)
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("CUSTOMER_NAME")]
        public string CustomerName { set; get; }


        /// <summary>
        ///請購單號
        /// </summary>
        /// 
        [Required]
        [Column("PR_NUMBER")]
        public long PrNumber { set; get; }

        /// <summary>
        ///請購明細行號
        /// </summary>
        /// 
        [Required]
        [Column("PR_LINE_NUMBER")]
        public long PrLineNumber { set; get; }

        /// <summary>
        ///請購明細ID
        /// </summary>
        /// 
        [Required]
        [Column("REQUISITION_LINE_ID")]
        public long RequisitionLineId { set; get; }

        /// <summary>
        ///採購單號
        /// </summary>
        /// 
        [Required]
        [Column("PO_NUMBER")]
        public long PoNumber { set; get; }



        /// <summary>
        ///採購明細行號
        /// </summary>
        /// 
        [Required]
        [Column("PO_LINE_NUMBER")]
        public long PoLineNumber { set; get; }

        /// <summary>
        ///採購明細ID
        /// </summary>
        /// 
        [Required]
        [Column("PO_LINE_ID")]
        public long PoLineId { set; get; }


        /// <summary>
        ///採購單價
        /// </summary>
        /// 
        [Required]
        [Column("PO_UNIT_PRICE")]
        public decimal PoUnitPrice { set; get; }

        /// <summary>
        ///採購單版次
        /// </summary>
        /// 
        [Required]
        [Column("PO_REVISION_NUM")]
        public long PoRevisionNum { set; get; }

        /// <summary>
        ///採購單狀態
        /// </summary>
        /// 
        [StringLength(25)]
        [Column("PO_STATUS")]
        public string PoStatus { set; get; }

        /// <summary>
        ///供應商編號
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("PO_VENDOR_NUM")]
        public string PoVendorNum { set; get; }

        /// <summary>
        ///委外工單備註
        /// </summary>
        /// 
        [Required]
        [StringLength(240)]
        [Column("OSP_REMARK")]
        public string OspRemark { set; get; }

        /// <summary>
        ///預留庫倉(組成成份)
        /// </summary>
        /// 
        [Required]
        [StringLength(20)]
        [Column("SUBINVENTORY")]
        public string Subinventory { set; get; }

        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }


        /// <summary>
        /// 儲位
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

        /// <summary>
        /// 預留單位(組成成份)
        /// </summary>
        [StringLength(03)]
        [Column("RESERVATION_UOM_CODE")]
        public string ReservationUomCode { set; get; }

        /// <summary>
        /// 預留數量(組成成份)
        /// </summary>
        [Column("RESERVATION_QUANTITY")]
        public decimal ReservationQuantity { set; get; }



        /// <summary>
        /// 建立人員(明細行)
        /// </summary>
        /// 
        [Required]
        [Column("LINE_CREATED_BY")]
        public long LineCreatedBy { set; get; }

        /// <summary>
        /// 建立日期(明細行)
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Required]
        [Column("LINE_CREATION_DATE")]
        public DateTime LineCreationDate { set; get; }

        /// <summary>
        /// 更新人員(明細行)
        /// </summary>
        /// 
        [Column("LINE_LAST_UPDATE_BY")]
        [Required]
        public long LineLastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間(明細行)
        /// </summary>
        /// 
        [Column("LINE_LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime LineLastUpdateDate { set; get; }

        /// <summary>
        /// 交易數量
        /// </summary>
        /// 
        [Required]
        [Column("TRANSACTION_QUANTITY")]
        public decimal TransactionQuantity { set; get; }

        /// <summary>
        /// 交易單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("TRANSACTION_UOM")]
        public string TransactionUom { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_QUANTITY")]
        public decimal PrimaryQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("PRIMARY_UOM")]
        public string PrimaryUom { set; get; }

        /// <summary>
        /// 次要數量
        /// </summary>
        /// 
        [Column("SECONDARY_QUANTITY")]
        public decimal? SecondaryQuantity { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("SECONDARY_UOM")]
        public string SecondaryUom { set; get; }


        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE1")]
        public string Attribute1 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE2")]
        public string Attribute2 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE3")]
        public string Attribute3 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE4")]
        public string Attribute4 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE5")]
        public string Attribute5 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE6")]
        public string Attribute6 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE7")]
        public string Attribute7 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE8")]
        public string Attribute8 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE9")]
        public string Attribute9 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE10")]
        public string Attribute10 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE11")]
        public string Attribute11 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE12")]
        public string Attribute12 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]

        [Column("ATTRIBUTE13")]
        public string Attribute13 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE14")]
        public string Attribute14 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE15")]
        public string Attribute15 { set; get; }

        /// <summary>
        /// ERP請求識別碼
        /// </summary>
        /// 
        [Column("REQUEST_ID")]
        public long? RequestId { set; get; }


        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [Required]
        [Column("CREATED_BY")]
        public long CreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Required]
        [Column("CREATION_DATE")]
        public DateTime CreationDate { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Column("LAST_UPDATE_BY")]
        [Required]
        public long LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [Column("LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime LastUpdateDate { set; get; }
    }
}
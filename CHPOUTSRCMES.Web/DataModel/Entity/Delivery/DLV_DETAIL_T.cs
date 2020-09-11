using CHPOUTSRCMES.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Delivery
{
    [Table("DLV_DETAIL_T")]
    public class DLV_DETAIL_T
    {
        /// <summary>
        /// 出庫明細ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("DLV_DETAIL_ID")]
        public long DlvDetailId { set; get; }

        /// <summary>
        /// 出庫檔頭ID
        /// </summary>
        /// 
        [Required]
        [Column("DLV_HEADER_ID")]
        public long DlvHeaderId { set; get; }

        /// <summary>
        /// XXIFP220
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("PROCESS_CODE")]
        public string ProcessCode { set; get; }


        /// <summary>
        /// 
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("SERVER_CODE")]
        public string ServerCode { set; get; }


        /// <summary>
        /// 20191112141600100000
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BATCH_ID")]
        public string BatchId { set; get; }


        /// <summary>
        /// 1
        /// </summary>
        /// 
        [Required]
        [Column("BATCH_LINE_ID")]
        public long BatchLineId { set; get; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        /// 
        [Required]
        [Column("Order_Number")]
        public long OrderNumber { set; get; }

        /// <summary>
        /// 訂單明細ID
        /// </summary>
        /// 
        [Required]
        [Column("ORDER_LINE_ID")]
        public long OrderLineId { set; get; }

        /// <summary>
        /// 訂單行號
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORDER_SHIP_NUMBER")]
        public string OrderShipNumber { set; get; }

        /// <summary>
        /// 出貨明細ID
        /// </summary>
        /// 
        [Required]
        [Column("DELIVERY_DETAIL_ID")]
        public long DeliveryDetailId { set; get; }

        /// <summary>
        /// 包裝方式
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }

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
        [Column("ITEM_NUMBER")]
        public string ItemNumber { set; get; }

        /// <summary>
        /// 料號名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESCRIPTION")]
        public string ItemDescription { set; get; }

        /// <summary>
        /// 令重
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("REAM_WEIGHT")]
        public string ReamWeight { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("ITEM_CATEGORY")]
        public string ItemCategory { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("PAPER_TYPE")]
        public string PaperType { set; get; }

        /// <summary>
        /// 基重
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("BASIC_WEIGHT")]
        public string BasicWeight { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
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
        /// 出貨儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }

        /// <summary>
        /// 出貨儲位
        /// </summary>
        /// 
        [StringLength(163)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

        /// <summary>
        /// 訂單原始交易數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_TRANSACTION_QUANTITY")]
        [Precision(30, 10)]
        public decimal SrcRequestedQuantity { set; get; }

        /// <summary>
        /// 訂單交易單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_TRANSACTION_UOM")]
        public string SrcRequestedQuantityUom { set; get; }

        /// <summary>
        /// 預計出庫主要數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_PRIMARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal RequestedQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_PRIMARY_UOM")]
        public string RequestedQuantityUom { set; get; }

        /// <summary>
        /// 預計出庫次要數量
        /// </summary>
        /// 
        [Column("REQUESTED_SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal? RequestedQuantity2 { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("REQUESTED_SECONDARY_UOM")]
        public string RequestedQuantityUom2 { set; get; }

        /// <summary>
        /// 文件ID(OPS工單ID)
        /// </summary>
        /// 
        [Column("OSP_BATCH_ID")]
        public long? OspBatchId { set; get; }

        /// <summary>
        /// 文件(工單號碼) 
        /// </summary>
        /// 
        [StringLength(32)]
        [Column("OSP_BATCH_NO")]
        public string OspBatchNo { set; get; }

        /// <summary>
        /// 工單類別
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("OSP_BATCH_TYPE")]
        public string OspBatchType { set; get; }

        /// <summary>
        /// 代紙料浩ID
        /// </summary>
        /// 
        [Column("TMP_ITEM_ID")]
        public long? TmpItemId { set; get; }

        /// <summary>
        /// 代紙料號 
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("TMP_ITEM_NUMBER")]
        public string TmpItemNumber { set; get; }

        /// <summary>
        /// 代紙料號名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("TMP_ITEM_DESCRIPTION")]
        public string TmpItemDescription { set; get; }


        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
        [Column("CREATED_BY")]
        public string CreatedBy { set; get; }

        /// <summary>
        /// 建立人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
        [Column("CREATED_USER_NAME")]
        public string CreatedUserName { set; get; }

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
        [StringLength(128)]
        [Column("LAST_UPDATE_BY")]
        public string LastUpdateBy { set; get; }

        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_USER_NAME")]
        public string LastUpdateUserName { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { set; get; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class CTR_PICKED_HT
    {

        /// <summary>
        /// 揀貨歷史ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("CTR_HIS_ID")]
        public long CtrHisId { set; get; }

        /// <summary>
        /// 入庫揀貨ID
        /// </summary>
        /// 
        [Required]
        [Column("CTR_PICKED_ID")]
        public long CtrPickedId { set; get; }


        /// <summary>
        /// 表頭ID
        /// </summary>
        /// 
        [Required]
        [Column("CTR_HEADER_ID")]
        public long CtrHeaderId { set; get; }

        /// <summary>
        /// 檔頭ID
        /// </summary>
        /// 
        [Required]
        [Column("CTR_DETAIL_ID")]
        public long CtrDetailId { set; get; }

        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [Column("STOCK_ID")]
        public long? StockId { set; get; }

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
        [StringLength(163)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

        /// <summary>
        /// 條碼
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BARCODE")]
        public string Barcode { set; get; }

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
        [Column("SHIP_ITEM_NUMBER")]
        public string ShipItemNumber { set; get; }

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
        /// 令重
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("REAM_WEIGHT")]
        public string ReamWeight { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 
        [Required]
        [Column("ROLL_REAM_WT")]
        public decimal RollReamWt { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("SPECIFICATION")]
        public string Specification { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }

        /// <summary>
        /// 出貨數量(MT)
        /// </summary>
        /// 
        [Column("SHIP_MT_QTY")]
        public decimal? ShipMtQty { set; get; }

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
        /// 捲號
        /// </summary>
        /// 
        [StringLength(80)]
        [Column("LOT_NUMBER")]
        public string LotNumber { set; get; }

        /// <summary>
        /// 理論重(批號數量)
        /// </summary>
        /// 
        [StringLength(80)]
        [Column("THEORY_WEIGHT")]
        [Required(AllowEmptyStrings = true)]
        public string TheoryWeight { set; get; }


        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("ITEM_CATEGORY")]
        public string ItemCategory { set; get; }


        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("STATUS")]
        public string Status { set; get; }

        /// <summary>
        /// 原因ID
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("REASON_CODE")]
        public string ReasonCode { set; get; }

        /// <summary>
        /// 原因說明
        /// </summary>
        /// 
        [StringLength(50)]
        [Column("REASON_DESC")]
        public string ReasonDesc { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(500)]
        [Column("NOTE")]
        public string Note { set; get; }


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
        [Required]
        [StringLength(128)]
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
        /// 更新時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { set; get; }


        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_USER_NAME")]
        public string LastUpdateUserName { set; get; }
    }
}
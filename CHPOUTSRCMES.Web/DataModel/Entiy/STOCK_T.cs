using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    public class STOCK_T
    {
        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("STOCK_ID")]
        public long StockId { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { set; get; }

        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("ORGANIZATION_CODE")]
        public string OrganizationCode { set; get; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }


        /// <summary>
        /// 儲位節段
        /// </summary>
        /// 
        [StringLength(163)]
        [Column("LOCATOR_SEGMENTS")]
        public string LocatorSegments { set; get; }

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
        /// 料號說明
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESCRIPTION")]
        public string ItemDescription { set; get; }



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
        /// 令重
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
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
        /// 工單號碼
        /// </summary>
        /// 
        [StringLength(32)]
        [Column("OSP_BATCH_NO")]
        public string OspBatchNo { set; get; }

        /// <summary>
        /// 捲號
        /// </summary>
        /// 
        [StringLength(80)]
        [Column("LOT_NUMBER")]
        public string LotNumber { set; get; }

        /// <summary>
        /// 條碼
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("BARCODE")]
        public string Barcode { set; get; }

        /// <summary>
        /// 主單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("PRIMARY_UOM_CODE")]
        public string PrimaryUomCode { set; get; }

        /// <summary>
        ///主單位原數量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_TRANSACTION_QTY")]
        public decimal PrimaryTransactionQty { set; get; }

        /// <summary>
        ///主單位可異動量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_AVAILABLE_QTY")]
        public decimal PrimaryAvailableQty { set; get; }

        /// <summary>
        ///主單位鎖定量
        /// </summary>
        /// 
        [Column("PRIMARY_LOCKED_QTY")]
        public decimal? PrimaryLockedQty { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("SECONDARY_UOM_CODE")]
        public string SecondaryUomCode { set; get; }


        /// <summary>
        /// 次單位原數量
        /// </summary>
        /// 
        [Column("SECONDARY_TRANSACTION_QTY")]
        public decimal? SecondaryTransactionQty { set; get; }


        /// <summary>
        /// 次單位可異動數量
        /// </summary>
        /// 
        [Column("SECONDARY_AVAILABLE_QTY")]
        public decimal? SecondaryAvailableQty { set; get; }

        /// <summary>
        ///次單位鎖定量
        /// </summary>
        /// 
        [Column("SECONDARY_LOCKED_QTY")]
        public decimal? SecondaryLockedQty { set; get; }

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
        [StringLength(2000)]
        [Column("NOTE")]
        public string Note { set; get; }


        /// <summary>
        /// 狀態碼
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("STATUS_CODE")]
        public string StatusCode { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
        [Column("CREATED_BY")]
        public string CreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [Required]
        [DataType(DataType.Date)]
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
        [Required]
        [DataType(DataType.Date)]
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { set; get; }

        public bool isRoll()
        {
            return this.ItemCategory.CompareTo("捲筒") == 0;
        }
    }
}
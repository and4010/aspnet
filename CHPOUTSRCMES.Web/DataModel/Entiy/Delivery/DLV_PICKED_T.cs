using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Delivery
{
    [Table("DLV_PICKED_T")]
    public class DLV_PICKED_T
    {
        /// <summary>
        /// 出庫揀貨ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("DLV_PICKED_ID")]
        public long DlvPickedId { set; get; }

        // <summary>
        /// 出庫明細ID
        /// </summary>
        /// 
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
        /// 庫存ID
        /// </summary>
        /// 
        [Required]
        [Column("STOCK_ID")]
        public long Stock_Id { set; get; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [StringLength(10)]
        [Column("STATUS")]
        public string Status { set; get; }

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
        [StringLength(30)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

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
        public string Item_Number { set; get; }

        /// <summary>
        /// 料號名稱
        /// </summary>
        /// 
        //[StringLength(240)]
        //[Required]
        //[Column("ITEM_DESCRIPTION")]
        //public string Item_Description { set; get; }

        /// <summary>
        /// 包裝方式
        /// </summary>
        /// 
        [StringLength(30)]
        [Required(AllowEmptyStrings = true)]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }

        /// <summary>
        /// 令重
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("REAM_WEIGHT")]
        public string ReamWeight { set; get; }

        /// <summary>
        /// 條碼號
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BARCODE")]
        public string Barcode { set; get; }

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
        [Required]
        [StringLength(3)]
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
        [Required]
        [StringLength(3)]
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
        public string Lot_Number { set; get; }

        /// <summary>
        /// 理論重(KG)
        /// </summary>
        /// 
        [Column("LOT_QUANTITY")]
        public long? LotQuantity { set; get; }

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
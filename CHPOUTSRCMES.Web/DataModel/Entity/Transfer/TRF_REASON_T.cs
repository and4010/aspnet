using CHPOUTSRCMES.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Transfer
{

    [Table("TRF_REASON_T")]
    public class TRF_REASON_T
    {
        /// <summary>
        /// 庫存移轉貨故ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("TRANSFER_REASON_ID")]
        public long TransferReasonId { set; get; }

        /// <summary>
        /// 庫存移轉貨故擋頭ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSFER_REASON_HEADER_ID")]
        public long TransferReasonHeaderId { set; get; }

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
        /// 條碼號
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BARCODE")]
        public string Barcode { set; get; }

        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [Required]
        [Column("STOCK_ID")]
        public long StockId { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("PRIMARY_UOM")]
        public string PrimaryUom { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal PrimaryQuantity { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("SECONDARY_UOM")]
        public string SecondaryUom { set; get; }

        /// <summary>
        /// 次要數量
        /// </summary>
        /// 
        [Column("SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal? SecondaryQuantity { set; get; }

        /// <summary>
        /// 捲號
        /// </summary>
        /// 
        [StringLength(80)]
        [Column("LOT_NUMBER")]
        public string LotNumber { set; get; }

        /// <summary>
        /// 理論重(KG)
        /// </summary>
        /// 
        [Column("LOT_QUANTITY")]
        [Precision(30, 10)]
        public decimal? LotQuantity { set; get; }

        /// <summary>
        /// 貨故原因ID
        /// </summary>
        [StringLength(10)]
        [Column("REASON_CODE")]
        public string ReasonCode { set; get; }

        /// <summary>
        /// 貨故原因說明
        /// </summary>
        [StringLength(50)]
        [Column("REASON_DESC")]
        public string ReasonDesc { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(240)]
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
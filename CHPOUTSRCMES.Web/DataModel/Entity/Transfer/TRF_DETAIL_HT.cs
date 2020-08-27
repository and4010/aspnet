using System;
using CHPOUTSRCMES.DataAnnotation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Transfer
{
    [Table("TRF_DETAIL_HT")]
    public class TRF_DETAIL_HT
    {
        /// <summary>
        /// 庫存移轉明細ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("TRANSFER_DETAIL_HIS_ID")]
        public long TransferDetailHisId { set; get; }

        /// <summary>
        /// 庫存移轉明細ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSFER_DETAIL_ID")]
        public long TransferDetailId { set; get; }

        /// <summary>
        /// 庫存移轉擋頭ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSFER_HEADER_ID")]
        public long TransferHeaderId { set; get; }

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
        /// 包裝方式
        /// </summary>
        /// 
        [StringLength(30)]
        [Required(AllowEmptyStrings = true)]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("ITEM_CATEGORY")]
        public string ItemCategory { set; get; }

        /// <summary>
        /// 交易單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_TRANSACTION_UOM")]
        public string RequestedTransactionUom { set; get; }

        /// <summary>
        /// 交易需求數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_TRANSACTION_QUANTITY")]
        [Precision(30, 10)]
        public decimal RequestedTransactionQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_PRIMARY_UOM")]
        public string RequestedPrimaryUom { set; get; }

        /// <summary>
        /// 主要需求數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_PRIMARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal RequestedPrimaryQuantity { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("REQUESTED_SECONDARY_UOM")]
        public string RequestedSecondaryUom { set; get; }

        /// <summary>
        /// 預計出庫次要數量
        /// </summary>
        /// 
        [Column("REQUESTED_SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal? RequestedSecondaryQuantity { set; get; }

        /// <summary>
        /// 捲數\棧板數
        /// </summary>
        /// 
        [Required]
        [Column("ROLL_REAM_QTY")]
        [Precision(30, 10)]
        public decimal RollReamQty { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 
        [Required]
        [Column("ROLL_REAM_WT")]
        [Precision(30, 10)]
        public decimal RollReamWt { set; get; }

        /// <summary>
        /// 修改權限
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("DATA_UPADTE_AUTHORITY")]
        public string DataUpadteAuthority { set; get; }

        /// <summary>
        /// 寫入方式
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("DATA_WRITE_TYPE")]
        public string DataWriteType { set; get; }

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
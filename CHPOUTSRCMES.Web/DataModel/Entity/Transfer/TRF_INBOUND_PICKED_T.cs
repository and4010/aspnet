using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CHPOUTSRCMES.DataAnnotation;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Transfer
{
    [Table("TRF_INBOUND_PICKED_T")]
    public class TRF_INBOUND_PICKED_T
    {
        /// <summary>
        /// 庫存移轉揀貨ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("TRANSFER_PICKED_ID")]
        public long TransferPickedId { set; get; }

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
        /// 庫存ID
        /// </summary>
        /// 
        [Required]
        [Column("STOCK_ID")]
        public long StockId { set; get; }


        /// <summary>
        /// 條碼號
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BARCODE")]
        public string Barcode { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_QUANTITY")]
        [Precision(30, 10)]
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
        [DisplayFormat(DataFormatString = "{0:0.##########}", ApplyFormatInEditMode = true)]
        [Column("SECONDARY_QUANTITY")]
        [Precision(30, 10)]
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
        /// 理論重(KG)
        /// </summary>
        /// 
        [Column("LOT_QUANTITY")]
        [Precision(30, 10)]
        public decimal? LotQuantity { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("NOTE")]
        public string Note { set; get; }

        /// <summary>
        /// 揀貨狀態
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("STATUS")]
        public string Status { set; get; }

        /// <summary>
        /// 棧板狀態
        /// </summary>
        /// 
        [Required]
        [StringLength(10)]
        [Column("PALLET_STATUS")]
        public string PalletStatus { set; get; }

        /// <summary>
        /// 拆至條碼
        /// </summary>
        /// 
        [StringLength(20)]
        [Column("SPLIT_FROM_BARCODE")]
        public string SplitFromBarcode { set; get; }

        /// <summary>
        /// 櫃號
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("CONTAINER_NO")]
        public string ContainerNo { set; get; }

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
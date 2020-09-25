﻿using CHPOUTSRCMES.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Process
{
    public class OSP_PICKED_IN_HT
    {
        /// <summary>
        /// 加工投入歷史明細ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_PICKED_IN_HT_ID")]
        public long OspPickedInHtId { set; get; }

        /// <summary>
        /// 加工投入明細ID
        /// </summary>
        /// 
        [Required]
        [Column("OSP_PICKED_IN_ID")]
        public long OspPickedInId { set; get; }


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
        /// 庫存ID
        /// </summary>
        /// 
        [Required]
        [Column("STOCK_ID")]
        public long StockId { set; get; }

        /// <summary>
        /// 條碼
        /// </summary>
        /// 
        [StringLength(40)]
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
        [Column("INVENTORY_ITEM_NUMBER")]
        public string InventoryItemNumber { set; get; }

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
        /// 捲號
        /// </summary>
        /// 
        [StringLength(80)]
        [Column("LOT_NUMBER")]
        [Required(AllowEmptyStrings = true)]
        public string LotNumber { set; get; }

        /// <summary>
        /// 理論重(KG)
        /// </summary>
        /// 
        [Column("LOT_QUANTITY")]
        [Precision(30, 10)]
        public decimal? LotQuantity { set; get; }

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
        /// 殘捲註記
        /// </summary>
        /// 
        [Column("HAS_REMAINT")]
        public string HasRemaint { set; get; }

        /// <summary>
        /// 剩餘重量
        /// </summary>
        /// 
        [Column("REMAINING_QUANTITY")]
        public decimal? RemainingQuantity { set; get; }

        /// <summary>
        /// 剩餘重量單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("REMAINING_UOM")]
        public string RemainingUom { set; get; }

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
        [StringLength(128)]
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
        [Column("LAST_UPDATE_USER_NAME")]
        [StringLength(128)]
        public string LastUpdateUserName { set; get; }
    }
}
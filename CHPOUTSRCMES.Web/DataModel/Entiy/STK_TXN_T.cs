using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    public class STK_TXN_T
    {
        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("STK_TXN_ID")]
        public int StkTxnId { set; get; }


        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [Required]
        [Column("STOCK_ID")]
        public int StockId { set; get; }


        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("ORGANIZATION_CODE")]
        public string OrganizationCode { set; get; }


        /// <summary>
        /// 庫存組織名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORGANIZATION_NAME")]
        public string OrganizationName { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }


        /// <summary>
        /// 倉庫名稱
        /// </summary>
        /// 
        [StringLength(50)]
        [Required]
        [Column("SUBINVENTORY_NAME")]
        public string SubinventoryName { set; get; }


        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Required]
        [Column("LOCATOR_ID")]
        public long LocatorId { set; get; }



        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("DST_ORGANIZATION_CODE")]
        public string DstOrganizationCode { set; get; }


        /// <summary>
        /// 庫存組織名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("DST_ORGANIZATION_NAME")]
        public string DstOrganizationName { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("DST_SUBINVENTORY_CODE")]
        public string DstSubinventoryCode { set; get; }


        /// <summary>
        /// 倉庫名稱
        /// </summary>
        /// 
        [StringLength(50)]
        [Required]
        [Column("DST_SUBINVENTORY_NAME")]
        public string DstSubinventoryName { set; get; }


        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Required]
        [Column("DST_LOCATOR_ID")]
        public long DstLocatorId { set; get; }


        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        /// 捲號
        /// </summary>
        /// 
        [StringLength(80)]
        [Required]
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
        /// 作業別
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("CATEGORY")]
        public string Category { set; get; }

        /// <summary>
        /// 單號
        /// </summary>
        /// 
        [StringLength(50)]
        [Required]
        [Column("DOC")]
        public string Doc { set; get; }

        /// <summary>
        /// 動作
        /// </summary>
        /// 
        [StringLength(50)]
        [Required]
        [Column("ACTION")]
        public string Action { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(250)]
        [Required]
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
        [Required]
        [Column("CREATED_BY")]
        public long CreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        [Column("CREATION_DATE")]
        public DateTime CreationDate { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Required]
        [Column("LAST_UPDATE_BY")]
        public long LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        [Column("LAST_UPDATE_DATE")]
        public DateTime LastUpdateDate { set; get; }
    }


}
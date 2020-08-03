using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("STK_TXN_T")]
    public class STK_TXN_T
    {
        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("STK_TXN_ID")]
        public long StkTxnId { set; get; }


        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
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


        ///// <summary>
        ///// 庫存組織名稱
        ///// </summary>
        ///// 
        //[StringLength(240)]
        //[Required]
        //[Column("ORGANIZATION_NAME")]
        //public string OrganizationName { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }


        ///// <summary>
        ///// 倉庫名稱
        ///// </summary>
        ///// 
        //[StringLength(50)]
        //[Required]
        //[Column("SUBINVENTORY_NAME")]
        //public string SubinventoryName { set; get; }


        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }

        /// <summary>
        /// 目的庫存組織ID
        /// </summary>
        /// 
        [Column("DST_ORGANIZATION_ID")]
        public long? DstOrganizationId { set; get; }

        /// <summary>
        /// 目的庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("DST_ORGANIZATION_CODE")]
        public string DstOrganizationCode { set; get; }


        ///// <summary>
        ///// 目的庫存組織名稱
        ///// </summary>
        ///// 
        //[StringLength(240)]
        //[Column("DST_ORGANIZATION_NAME")]
        //public string DstOrganizationName { set; get; }


        /// <summary>
        /// 目的倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("DST_SUBINVENTORY_CODE")]
        public string DstSubinventoryCode { set; get; }


        ///// <summary>
        ///// 目的倉庫名稱
        ///// </summary>
        ///// 
        //[StringLength(50)]
        //[Column("DST_SUBINVENTORY_NAME")]
        //public string DstSubinventoryName { set; get; }


        /// <summary>
        /// 目的儲位ID
        /// </summary>
        /// 
        [Column("DST_LOCATOR_ID")]
        public long? DstLocatorId { set; get; }


        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        [Required]
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
        [Column("PRY_UOM_CODE")]
        public string PryUomCode { set; get; }

        /// <summary>
        ///主單位原數量
        /// </summary>
        /// 
        [Required]
        [Column("PRY_BEF_QTY")]
        public decimal PryBefQty { set; get; }

        /// <summary>
        ///主單位異動後數量
        /// </summary>
        /// 
        [Column("PRY_AFT_QTY")]
        public decimal PryAftQty { set; get; }

        /// <summary>
        ///主單位異動量
        /// </summary>
        /// 
        [Column("PRY_CHG_QTY")]
        public decimal PryChgQty { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("SEC_UOM_CODE")]
        public string SecUomCode { set; get; }


        /// <summary>
        /// 次單位原數量
        /// </summary>
        /// 
        [Column("SEC_BEF_QTY")]
        public decimal? SecBefQty { set; get; }


        /// <summary>
        ///次單位異動量
        /// </summary>
        /// 
        [Column("SEC_CHG_QTY")]
        public decimal? SecChgQty { set; get; }
        

        /// <summary>
        /// 次單位可異動數量
        /// </summary>
        /// 
        [Column("SEC_AFT_QTY")]
        public decimal? SecAftQty { set; get; }


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
    }


}
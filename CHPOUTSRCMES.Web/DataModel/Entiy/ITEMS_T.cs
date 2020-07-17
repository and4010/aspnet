using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("ITEMS_T")]
    public class ITEMS_T
    {


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

        ///// <summary>
        ///// 庫存組織CODE
        ///// </summary>
        ///// 
        //[StringLength(3)]
        //[Required]
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Column("ORGANIZATION_CODE")]
        //public string OrganizationCode { set; get; }

        /// <summary>
        /// 存貨分類
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required]
        [Column("CATEGORY_CODE_INV")]
        public string CategoryCodeInv { set; get; }

        /// <summary>
        /// 存貨分類摘要
        /// </summary>
        /// 
        [StringLength(13)]
        [Required]
        [Column("CATEGORY_NAME_INV")]
        public string CategoryNameInv { set; get; }

        /// <summary>
        /// 成本分類
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATEGORY_CODE_COST")]
        public string CategoryCodeCost { set; get; }

        /// <summary>
        /// 成本分類摘要
        /// </summary>
        /// 
        [StringLength(8)]
        [Required]
        [Column("CATEGORY_NAME_COST")]
        public string CategoryNameCost { set; get; }

        /// <summary>
        /// 控制分類
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATEGORY_CODE_CONTROL")]
        public string CategoryCodeControl { set; get; }

        /// <summary>
        /// 控制分類摘要
        /// </summary>
        /// 
        [StringLength(16)]
        [Required]
        [Column("CATEGORY_NAME_CONTROL")]
        public string CategoryNameControl { set; get; }


        /// <summary>
        /// 英文中文摘要
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESC_ENG")]
        public string ItemDescEng { set; get; }

        /// <summary>
        /// 檢體中文摘要
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESC_SCH")]
        public string ItemDescSch { set; get; }

        /// <summary>
        /// 繁體中文摘要
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESC_TCH")]
        public string ItemDescTch { set; get; }


        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("PRIMARY_UOM_CODE")]
        public string PrimaryUomCode { set; get; }


        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required(AllowEmptyStrings = true)]
        [Column("SECONDARY_UOM_CODE")]
        public string SecondaryUomCode { set; get; }

        /// <summary>
        /// 料號狀態
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("INVENTORY_ITEM_STATUS_CODE")]
        public string InventoryItemStatusCode { set; get; }

        /// <summary>
        /// 料號型態
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("ITEM_TYPE")]
        public string ItemType { set; get; }


        /// <summary>
        /// 大紙別
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_010")]
        public string CatalogElemVal010 { set; get; }

        /// <summary>
        /// 紙別代碼
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_020")]
        public string CatalogElemVal020 { set; get; }

        /// <summary>
        /// 料件等級
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_030")]
        public string CatalogElemVal030 { set; get; }

        /// <summary>
        /// 基重
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_040")]
        public string CatalogElemVal040 { set; get; }

        /// <summary>
        /// 規格  紙捲= 寬幅 
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_050")]
        public string CatalogElemVal050 { set; get; }

        /// <summary>
        /// 令重
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_060")]
        public string CatalogElemVal060 { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_070")]
        public string CatalogElemVal070 { set; get; }

        /// <summary>
        /// FSC
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_080")]
        public string CatalogElemVal080 { set; get; }

        /// <summary>
        /// 市場常規
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_090")]
        public string CatalogElemVal090 { set; get; }

        /// <summary>
        /// 絲向
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_100")]
        public string CatalogElemVal100 { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_110")]
        public string CatalogElemVal110 { set; get; }

        /// <summary>
        /// 市銷\常規
        /// </summary>
        /// 
        [StringLength(4000)]
        [Column("CATALOG_ELEM_VAL_120")]
        public string CatalogElemVal120 { set; get; }

        /// <summary>
        /// 紙別中文名稱
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_130")]
        public string CatalogElemVal130 { set; get; }

        /// <summary>
        /// 紙別英文名稱
        /// </summary>
        /// 
        [StringLength(4000)]
        [Required(AllowEmptyStrings = true)]
        [Column("CATALOG_ELEM_VAL_140")]
        public string CatalogElemVal140 { set; get; }

        /// <summary>
        /// D:刪除
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [Column("CONTROL_FLAG",TypeName = "char")]
        [StringLength(1)]
        public string ControlFlag { set; get; }



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
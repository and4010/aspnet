using System;

namespace CHPOUTSRCMES.TASK.Models.Entity
{
    public class ITEMS_T
    {


        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        public long INVENTORY_ITEM_ID { set; get; }

        /// <summary>
        /// 料號
        /// </summary>
        /// 
        public string ITEM_NUMBER { set; get; }

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
        public string CATEGORY_CODE_INV { set; get; }

        /// <summary>
        /// 存貨分類摘要
        /// </summary>
        /// 
        public string CATEGORY_NAME_INV { set; get; }

        /// <summary>
        /// 成本分類
        /// </summary>
        /// 
        public string CATEGORY_CODE_COST { set; get; }

        /// <summary>
        /// 成本分類摘要
        /// </summary>
        /// 
        public string CATEGORY_NAME_COST { set; get; }

        /// <summary>
        /// 控制分類
        /// </summary>
        /// 
        public string CATEGORY_CODE_CONTROL { set; get; }

        /// <summary>
        /// 控制分類摘要
        /// </summary>
        /// 
        public string CATEGORY_NAME_CONTROL { set; get; }


        /// <summary>
        /// 英文中文摘要
        /// </summary>
        /// 
        public string ITEM_DESC_ENG { set; get; }

        /// <summary>
        /// 檢體中文摘要
        /// </summary>
        /// 
        public string ITEM_DESC_SCH { set; get; }

        /// <summary>
        /// 繁體中文摘要
        /// </summary>
        /// 
        public string ITEM_DESC_TCH { set; get; }


        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        public string PRIMARY_UOM_CODE { set; get; }


        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        public string SECONDARY_UOM_CODE { set; get; }

        /// <summary>
        /// 料號狀態
        /// </summary>
        /// 
        public string INVENTORY_ITEM_STATUS_CODE { set; get; }

        /// <summary>
        /// 料號型態
        /// </summary>
        /// 
        public string ITEM_TYPE { set; get; }


        /// <summary>
        /// 大紙別
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_010 { set; get; }

        /// <summary>
        /// 紙別代碼
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_020 { set; get; }

        /// <summary>
        /// 料件等級
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_030 { set; get; }

        /// <summary>
        /// 基重
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_040 { set; get; }

        /// <summary>
        /// 規格  紙捲= 寬幅 
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_050 { set; get; }

        /// <summary>
        /// 令重
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_060 { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_070 { set; get; }

        /// <summary>
        /// FSC
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_080 { set; get; }

        /// <summary>
        /// 市場常規
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_090 { set; get; }

        /// <summary>
        /// 絲向
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_100 { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_110 { set; get; }

        /// <summary>
        /// 市銷\常規
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_120 { set; get; }

        /// <summary>
        /// 紙別中文名稱
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_130 { set; get; }

        /// <summary>
        /// 紙別英文名稱
        /// </summary>
        /// 
        public string CATALOG_ELEM_VAL_140 { set; get; }

        /// <summary>
        /// D:刪除
        /// </summary>
        /// 
        public string CONTROL_FLAG { set; get; }



        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        public long CREATED_BY { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        public DateTime CREATION_DATE { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        public long LAST_UPDATED_BY { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        public DateTime LAST_UPDATED_DATE { set; get; }
    }
}
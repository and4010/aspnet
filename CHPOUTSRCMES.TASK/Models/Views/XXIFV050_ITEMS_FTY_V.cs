using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Views
{
    public class XXIFV050_ITEMS_FTY_V
    {
        public string ORGANIZATION_CODE { set; get; }
        public string CATEGORY_CODE_INV { set; get; }
        public string CATEGORY_NAME_INV { set; get; }
        public string CATEGORY_CODE_COST { set; get; }
        public string CATEGORY_NAME_COST { set; get; }
        public string CATEGORY_CODE_CONTROL { set; get; }
        public string CATEGORY_NAME_CONTROL { set; get; }
        public string ITEM_NUMBER { set; get; }
        public long INVENTORY_ITEM_ID { set; get; }
        public string ITEM_DESC_ENG { set; get; }
        public string ITEM_DESC_SCH { set; get; }
        public string ITEM_DESC_TCH { set; get; }
        public string PRIMARY_UOM_CODE { set; get; }
        public string SECONDARY_UOM_CODE { set; get; }
        public string INVENTORY_ITEM_STATUS_CODE { set; get; }
        public string ITEM_TYPE { set; get; }
        public string CATALOG_ELEM_VAL_010 { set; get; }
        public string CATALOG_ELEM_VAL_020 { set; get; }
        public string CATALOG_ELEM_VAL_030 { set; get; }
        public string CATALOG_ELEM_VAL_040 { set; get; }
        public string CATALOG_ELEM_VAL_050 { set; get; }
        public string CATALOG_ELEM_VAL_060 { set; get; }
        public string CATALOG_ELEM_VAL_070 { set; get; }
        public string CATALOG_ELEM_VAL_080 { set; get; }
        public string CATALOG_ELEM_VAL_090 { set; get; }
        public string CATALOG_ELEM_VAL_100 { set; get; }
        public string CATALOG_ELEM_VAL_110 { set; get; }
        public string CATALOG_ELEM_VAL_120 { set; get; }
        public string CATALOG_ELEM_VAL_130 { set; get; }
        public string CATALOG_ELEM_VAL_140 { set; get; }

        /// <summary>
        /// 建立人員ID
        /// </summary>
        public long CREATED_BY { set; get; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CREATION_DATE { set; get; }
        /// <summary>
        /// 最後更新人員ID
        /// </summary>
        public long LAST_UPDATED_BY { set; get; }
        /// <summary>
        /// 最後更新日期
        /// </summary>
        public DateTime LAST_UPDATE_DATE { set; get; }
    }
}

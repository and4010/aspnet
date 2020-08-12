namespace CHPOUTSRCMES.TASK.Models.Entity.Shadowed
{
    public class SUBINVENTORY_SHADOWED_T
    {

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        public long ORGANIZATION_ID { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        public string SUBINVENTORY_CODE { set; get; }


        /// <summary>
        /// 倉庫名稱
        /// </summary>
        /// 
        public string SUBINVENTORY_NAME { set; get; }

        /// <summary>
        /// 儲位控制
        /// </summary>
        /// 
        public long LOCATOR_TYPE { set; get; }

        /// <summary>
        /// 加工廠註記
        /// </summary>
        /// 
        public string OSP_FLAG { set; get; }


        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        public string CONTROL_FLAG { set; get; }
    }
}
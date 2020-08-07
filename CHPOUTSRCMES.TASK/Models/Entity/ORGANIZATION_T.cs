namespace CHPOUTSRCMES.TASK.DataModel.Entiy.Information
{
    public class ORGANIZATION_T
    {

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        public long ORGANIZATION_ID { set; get; }



        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        public string ORGANIZATION_CODE { set; get; }


        /// <summary>
        /// 庫存組織名稱
        /// </summary>
        /// 
        public string ORGANIZATION_NAME { set; get; }



        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        public string CONTROL_FLAG { set; get; }

    }
}
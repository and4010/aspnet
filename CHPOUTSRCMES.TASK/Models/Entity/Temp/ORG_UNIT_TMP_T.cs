namespace CHPOUTSRCMES.TASK.Models.Entity.Temp
{
    public class ORG_UNIT_TMP_T
    {

        /// <summary>
        /// 作業單元ID
        /// </summary>
        /// 
        public long ORG_ID { set; get; }


        /// <summary>
        /// 作業單元名稱
        /// </summary>
        /// 
        public string ORG_NAME { set; get; }


        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        public string CONTROL_FLAG { set; get; }
    }
}
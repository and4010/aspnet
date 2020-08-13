using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Temp
{
    public class ORG_UNIT_TMP_T
    {

        /// <summary>
        /// 作業單元ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORG_ID")]
        public long OrgId { set; get; }


        /// <summary>
        /// 作業單元
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORG_NAME")]
        public string OrgName { set; get; }


        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        [StringLength(1)]
        [Required(AllowEmptyStrings = true)]
        [Column("CONTROL_FLAG", TypeName = "char")]
        public string ControlFlag { set; get; }

    }
}
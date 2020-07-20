using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Information
{
    public class SUBINVENTORY_T
    {

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORGANIZATION_ID",Order = 1)]
        public long OrganizationID { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SUBINVENTORY_CODE", Order = 2)]
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
        /// 加工廠註記
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("OSP_FLAG")]
        public string OspFlag { set; get; }


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
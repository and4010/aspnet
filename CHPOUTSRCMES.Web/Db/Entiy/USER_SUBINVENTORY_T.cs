using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.Db.Entiy
{
    public class USER_SUBINVENTORY_T
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(128)]
        [Column("UserId")]
        public string UserId { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [Column("ORGANIZATION_ID")]
        public long OrganizationID { set; get; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(10)]
        [Column("SUBINVENTORY_CODE")]
        public long SUBINVENTORY_CODE { set; get; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Information
{
    public class USER_SUBINVENTORY_T
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(128)]
        [Column("UserId", Order = 1)]
        public string UserId { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORGANIZATION_ID", Order = 2)]
        public long OrganizationID { set; get; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        [Column("SUBINVENTORY_CODE", Order = 3)]
        public string SUBINVENTORY_CODE { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Entiy
{
    public class BCD_PREFIX_T
    {

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORGANIZATION_ID")]
        public long OrganizationID { set; get; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [Required]
        [Key]
        [Column("SUBINVENTORY_CODE")]
        public long SsubinventoryCode { set; get; }

        /// <summary>
        /// 條碼前置碼
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("PREFIX_CODE")]
        public string PrefixCode { set; get; }

    }
}
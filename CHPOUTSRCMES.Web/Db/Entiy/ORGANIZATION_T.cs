using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Entiy
{
    public class ORGANIZATION_T
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
        /// 庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("ORGANIZATION_CODE")]
        public string OrganizationCode { set; get; }


        /// <summary>
        /// 庫存組織名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORGANIZATION_NAME")]
        public string OrganizationName { set; get; }

    }
}
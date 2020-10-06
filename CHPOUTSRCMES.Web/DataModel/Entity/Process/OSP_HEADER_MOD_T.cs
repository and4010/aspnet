using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_HEADER_MOD_T
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

        /// <summary>
        /// XXIFP217
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORG_OSP_HEADER_ID", Order = 2)]
        public long OrgOspHeaderId { set; get; }

    }
}
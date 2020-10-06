using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_SOA_DTL_S2_T
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OspSoaDtlS2Id { set; get; }

        [Required]
        public long OspSoaS2Id { set; get; }

        [Required]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

    }
}
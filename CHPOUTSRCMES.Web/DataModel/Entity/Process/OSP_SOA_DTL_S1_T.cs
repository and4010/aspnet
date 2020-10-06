using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_SOA_DTL_S1_T
    {
        /// <summary>
        /// RXD
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_SOA_DTL_S1_ID")]
        public long OspSoaDtlS1Id { set; get; }

        /// <summary>
        /// OSP_SOA_S1_ID
        /// </summary>
        [Required]
        [Column("OSP_SOA_S1_ID")]
        public long OspSoaS1Id { set; get; }

        [StringLength(20)]
        [Required]
        [Column("BATCH_LINE_ID")]
        public string BatchLineId { set; get; }

        [Required]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

    }
}
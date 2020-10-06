using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_SOA_DTL_S3_T
    {
        /// <summary>
        /// RXD
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OspSoaDtlS3Id { set; get; }

        /// <summary>
        /// OSP_SOA_S3_ID
        /// </summary>
        [Required]
        public long OspSoaS3Id { set; get; }

        [StringLength(20)]
        [Required]
        public string BATCH_LINE_ID { set; get; }

        [Required]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

    }
}
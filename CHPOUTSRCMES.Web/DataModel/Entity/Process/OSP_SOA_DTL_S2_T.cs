using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_SOA_DTL_S2_T
    {
        /// <summary>
        /// RXD
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_SOA_DTL_S2_ID")]
        public long OspSoaDtlS2Id { set; get; }

        /// <summary>
        /// OSP_SOA_S2_ID
        /// </summary>
        [Required]
        [Column("OSP_SOA_S2_ID")]
        public long OspSoaS2Id { set; get; }

        [Required]
        [Column("INVENTORY_ITEM_ID")]
        public long InventoryItemId { set; get; }

        [Required]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

    }
}
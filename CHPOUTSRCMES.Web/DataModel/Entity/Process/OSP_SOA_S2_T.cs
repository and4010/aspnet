using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class OSP_SOA_S2_T
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_SOA_S2_ID")]
        public long OspSoaS2Id { set; get; }

        [Required]
        [Column("OSP_HEADER_ID", Order = 1)]
        public long OspHeaderId { set; get; }

        /// <summary>
        /// XXIFP217
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("PROCESS_CODE", Order = 2)]
        public string ProcessCode { set; get; }


        /// <summary>
        /// 
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("SERVER_CODE", Order = 3)]
        public string ServerCode { set; get; }


        /// <summary>
        /// 20191112141600100000
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BATCH_ID", Order = 4)]
        public string BatchId { set; get; }

        /// <summary>
        /// 狀態碼
        /// </summary>
        [StringLength(1)]
        [Column("STATUS_CODE")]
        public string StatusCode { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [Required]
        [StringLength(128)]
        [Column("CREATED_BY")]
        public string CreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Required]
        [Column("CREATION_DATE")]
        public DateTime CreationDate { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Column("LAST_UPDATE_BY")]
        [StringLength(128)]
        public string LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [Column("LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdateDate { set; get; }

    }
}
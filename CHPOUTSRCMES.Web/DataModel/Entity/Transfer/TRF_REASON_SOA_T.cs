using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Transfer
{

    [Table("TRF_REASON_SOA_T")]
    public class TRF_REASON_SOA_T
    {
        /// <summary>
        /// 庫存移轉貨故檔頭ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("TRANSFER_REASON_SOA_ID", Order = 1)]
        public long TransferReasonHeaderId { set; get; }



        /// <summary>
        /// XXIFP217
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("PROCESS_CODE", Order = 2)]
        public string ProcessCode { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SERVER_CODE", Order = 3)]
        public string ServerCode { set; get; }


        /// <summary>
        /// 20191112141600100000
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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


    }
}
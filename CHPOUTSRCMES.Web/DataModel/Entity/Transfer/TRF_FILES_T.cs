using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CHPOUTSRCMES.Web.DataModel.Entity.Transfer
{
    [Table("TRF_FILES_T")]
    public class TRF_FILES_T
    {
        /// <summary>
        /// 檔案ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TRF_FILE_ID")]
        public long TrfFileId { set; get; }


        /// <summary>
        /// 檔案實體
        /// </summary>
        /// 
        [Column("FILE_INSTANCE")]
        [Required]
        public Byte[] FileInstance { set; get; }
    }
}
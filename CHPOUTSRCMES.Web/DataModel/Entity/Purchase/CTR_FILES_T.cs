using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class CTR_FILES_T
    {

        /// <summary>
        /// 檔案ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CTR_FILE_ID")]
        public long CtrFileId { set; get; }


        /// <summary>
        /// 檔案實體
        /// </summary>
        /// 
        [Column("FILE_INSTANCE")]
        [Required]
        public Byte[] FileInstance { set; get; }
    }
}
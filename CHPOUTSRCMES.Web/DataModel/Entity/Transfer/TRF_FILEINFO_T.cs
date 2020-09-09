using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CHPOUTSRCMES.Web.DataModel.Entity.Transfer
{
    [Table("TRF_FILEINFO_T")]
    public class TRF_FILEINFO_T
    {
        /// <summary>
        /// 檔案資訊 ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("TRF_FILEINFO_ID")]
        public long TrfFileinfoId { set; get; }

        /// <summary>
        /// 庫存移轉貨故ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSFER_REASON_ID")]
        public long TransferReasonId { set; get; }

        /// <summary>
        /// 庫存移轉貨故擋頭ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSFER_REASON_HEADER_ID")]
        public long TransferReasonHeaderId { set; get; }



        /// <summary>
        /// 檔案ID
        /// </summary>
        /// 
        [Required]
        [Column("TRF_FILE_ID")]
        public long TrfFileId { set; get; }


        /// <summary>
        /// 檔案類型
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("FILE_TYPE")]
        public string FileType { set; get; }


        /// <summary>
        /// 檔案名稱
        /// </summary>
        /// 
        [StringLength(250)]
        [Required]
        [Column("FILENAME")]
        public string FileName { set; get; }

        /// <summary>
        /// 檔案大小
        /// </summary>
        /// 
        [Required]
        [Column("SIZE")]
        public long Size { set; get; }

        /// <summary>
        /// 項次
        /// </summary>
        /// 
        [Required]
        [Column("SEQ")]
        public long Seq { set; get; }


        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
        [Column("CREATED_BY")]
        public string CreatedBy { set; get; }

        /// <summary>
        /// 建立人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
        [Column("CREATED_USER_NAME")]
        public string CreatedUserName { set; get; }

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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Entiy.Purchase
{
    public class CTR_FILEINFO_T
    {
        /// <summary>
        /// 檔案資訊 ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("CTR_FILEINFO_ID")]
        public long CtrFileinfoId { set; get; }

        /// <summary>
        /// 入庫揀貨ID
        /// </summary>
        /// 
        [Required]
        [Column("CTR_PICKED_ID")]
        public long CtrPickedId { set; get; }

        /// <summary>
        /// 檔案ID
        /// </summary>
        /// 
        [Required]
        [Column("CTR_FILE_ID")]
        public long CtrFileId { set; get; }


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
        [Required]
        [Column("CREATED_BY")]
        public long CreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        [Column("CREATION_DATE")]
        public DateTime CreationDate { set; get; }


    }
}
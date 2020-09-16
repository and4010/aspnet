using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Information
{
    [Table("DOC_UNIQUE_T")]
    public class DOC_UNIQUE_T
    {
        /// <summary>
        /// 前置碼
        /// </summary>
        /// 
        [Required]
        [StringLength(30)]
        [Index("DOC_UNIQUE_T_IDX1", Order = 1)]
        [Column("DOC_PREFIX")]
        public string Prefix { set; get; }
        
        /// <summary>
        /// 流水號
        /// </summary>
        [Required]
        [Index("DOC_UNIQUE_T_IDX1", Order = 2)]
        [Column("DOC_SEQ")]
        public int Seq { set; get; }

        [Required]
        [Key]
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("DOC_NO")]
        public string DocNo { set; get; }

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Required]
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
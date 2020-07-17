using CHPOUTSRCMES.Web.DataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("STOCK_REASON_T")]
    public class STOCK_REASON_T
    {
        /// <summary>
        /// 原因ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        [Required]
        [Key]
        [Column("REASON_CODE")]
        public string ReasonCode { set; get; }

        /// <summary>
        /// 原因說明
        /// </summary>
        /// 
        [StringLength(50)]
        [Required]
        [Column("REASON_DESC")]
        public string ReasonDesc { set; get; }

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

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Column("LAST_UPDATE_BY")]
        [Required]
        public long LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [Column("LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime LastUpdateDate { set; get; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Information
{
    [Table("BCD_UNIQUE_T")]
    public class BCD_UNIQUE_T
    {
        /// <summary>
        /// 條碼
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("BARCODE")]
        public string Barcode { set; get; }

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
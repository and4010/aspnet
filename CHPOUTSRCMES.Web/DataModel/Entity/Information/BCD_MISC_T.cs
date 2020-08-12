using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Information
{
    [Table("BCD_MISC_T")]
    public class BCD_MISC_T
    {
        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORGANIZATION_ID", Order= 1)]
        public long OrganizationId { set; get; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Key]
        [Column("SUBINVENTORY_CODE", Order=2)]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 前置碼
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("PREFIX_CODE")]
        public string PrefixCode { set; get; }

        /// <summary>
        /// 流水號碼數
        /// </summary>
        /// 
        [Required]
        [Column("SERIAL_SIZE")]
        public int SerialSize { set; get; }

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

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_BY")]
        public string LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { set; get; }
    }
}
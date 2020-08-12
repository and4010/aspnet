using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Information
{
    [Table("BCD_SERIAL_T")]
    public class BCD_SERIAL_T
    {
        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("BCD_DATE", Order= 1)]
        public string BcdDate { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ORGANIZATION_ID", Order = 2)]
        public long OrganizationId { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("SUBINVENTORY_CODE", Order = 3)]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 前置碼
        /// </summary>
        /// 
        [Required]
        [Key]
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("PREFIX_CODE", Order = 4)]
        public string PrefixCode { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Column("SERIAL_NUMBER")]
        public long SerialNumber { set; get; }

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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("LOCATOR_T")]
    public class LOCATOR_T
    {

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("LOCATOR_ID",Order = 1)]
        public long LocatorId { set; get; }


        /// <summary>
        /// 儲位控制
        /// </summary>
        /// 
        [Required]
        [Column("LOCATOR_TYPE")]
        public long LocatorType { set; get; }


        /// <summary>
        /// 儲位節段
        /// </summary>
        /// 
        [StringLength(163)]
        [Required]
        [Column("LOCATOR_SEGMENTS")]
        public string LocatorSegments { set; get; }


        /// <summary>
        /// 儲位描述
        /// </summary>
        /// 
        [StringLength(50)]
        [Column("LOCATOR_DESC")]
        public string LocatorDesc { set; get; }

        /// <summary>
        /// 儲位第一節段
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("SEGMENT1")]
        public string Segment1 { set; get; }

        /// <summary>
        /// 儲位第二節段
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("SEGMENT2")]
        public string Segment2 { set; get; }

        /// <summary>
        /// 儲位第三節段
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("SEGMENT3")]
        public string Segment3 { set; get; }

        /// <summary>
        /// 儲位第四節段
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("SEGMENT4")]
        public string Segment4 { set; get; }
    }
}
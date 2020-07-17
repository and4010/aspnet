using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Entiy
{
    public class STOCK_T
    {
        /// <summary>
        /// 庫存ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("STOCK_ID")]
        public int StockId { set; get; }

        /// <summary>
        /// 庫存組織ID
        /// </summary>
        /// 
        [Required]
        [Column("ORGANIZATION_ID")]
        public int OrganizationId { set; get; }


        /// <summary>
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SsubinventoryCode { set; get; }

        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Required]
        [Column("LOCATOR_ID")]
        public int LocatorId { set; get; }


        /// <summary>
        /// 儲位節段
        /// </summary>
        /// 
        [StringLength(163)]
        [Required]
        [Column("LOCATOR_SEGMENTS")]
        public string LocatorSegments { set; get; }

    }
}
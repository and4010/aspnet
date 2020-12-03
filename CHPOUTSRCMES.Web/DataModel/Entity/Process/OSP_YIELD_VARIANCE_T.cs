using CHPOUTSRCMES.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Process
{
    public class OSP_YIELD_VARIANCE_T
    {

        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_YIELD_VARIANCE_ID")]
        public long OspYieldVarianceId { set; get; }

        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        [Required]
        [Column("OSP_HEADER_ID")]
        public long OspHeaderId { set; get; }

        /// <summary>
        /// 投入料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("DETAIL_IN_ITEM_NUMBER")]
        public string DetailInItemNumber { set; get; }

        /// <summary>
        /// 投入重量
        /// </summary>
        [Column("DETAIL_IN_QUANTITY")]
        [Precision(30, 10)]
        public decimal DetailInQuantity { set; get; }

        /// <summary>
        /// 投入次要數量
        /// </summary>
        [Column("DETAIL_IN_SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal DetailInSecondaryQuantity { set; get; }

        
        /// <summary>
        /// 投入主要單位
        /// </summary>
        [StringLength(03)]
        [Column("DETAIL_IN_PRIMARY_UOM")]
        public string DetailInPrimaryUom { set; get; }

        /// <summary>
        /// 投入次要單位
        /// </summary>
        [StringLength(03)]
        [Column("DETAIL_IN_SECONDARY_UOM")]
        public string DetailInSecondaryUom { set; get; }

        /// <summary>
        /// 產出料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("DETAIL_OUT_ITEM_NUMBER")]
        public string DetailOutItemNumber { set; get; }

        /// <summary>
        /// 總產出重量(產出+餘切)
        /// </summary>
        [Column("DETAIL_OUT_QUANTITY")]
        [Precision(30, 10)]
        public decimal DetailOutQuantity { set; get; }

        /// <summary>
        /// 產出數量
        /// </summary>
        [Column("DETAIL_OUT_PRIMARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal DetailOutPrimaryQuantity { set; get; }

        /// <summary>
        /// 產出次要數量
        /// </summary>
        [Column("DETAIL_OUT_SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal DetailOutSecondaryQuantity { set; get; }

        /// <summary>
        /// 產出主要單位
        /// </summary>
        [StringLength(03)]
        [Column("DETAIL_OUT_PRIMARY_UOM")]
        public string DetailOutPrimaryUom { set; get; }

        /// <summary>
        /// 產出次要單位
        /// </summary>
        [StringLength(03)]
        [Column("DETAIL_OUT_SECONDARY_UOM")]
        public string DetailOutSecondaryUom { set; get; }

        /// <summary>
        /// 餘切料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("COTANGENT_ITEM_NUMBER")]
        public string CotangentItemNumber { set; get; }

        /// <summary>
        /// 餘切重量
        /// </summary>
        [Column("COTANGENT_QUANTITY")]
        [Precision(30, 10)]
        public decimal CotangentQuantity { set; get; }

        /// <summary>
        /// 餘切次要數量
        /// </summary>
        [Column("COTANGENT_SECONDARY_QUANTITY")]
        [Precision(30, 10)]
        public decimal CotangentSecondaryQuantity { set; get; }

        /// <summary>
        /// 餘切主要單位
        /// </summary>
        [StringLength(03)]
        [Column("COTANGENT_PRIMARY_UOM")]
        public string CotangentPrimaryUom { set; get; }

        /// <summary>
        /// 餘切次要單位
        /// </summary>
        [StringLength(03)]
        [Column("COTANGENT_SECONDARY_UOM")]
        public string CotangentSecondaryUom { set; get; }

        /// <summary>
        /// 損耗重量
        /// </summary>
        [Column("LOSS_WEIGHT")]
        public decimal LossWeight { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        //[StringLength(03)]
        //[Column("PRIMARY_UOM")]
        //public string PrimaryUom { set; get; }


        /// <summary>
        /// 得率
        /// </summary>
        [Column("RATE")]
        public decimal Rate { set; get; }


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
        [Required]
        [StringLength(128)]
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


        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_USER_NAME")]
        public string LastUpdateUserName { set; get; }
    }
}
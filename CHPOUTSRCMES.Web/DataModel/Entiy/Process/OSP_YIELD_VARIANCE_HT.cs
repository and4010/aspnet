using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Process
{
    public class OSP_YIELD_VARIANCE_HT
    {
        /// <summary>
        /// 加工損耗揀貨歷史ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_YIELD_VARIANCE_HT_ID")]
        public long OspYieldVarianceHtId { set; get; }

        /// <summary>
        ///  加工損耗揀貨ID
        /// </summary>
        /// 
        [Required]
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
        /// 投入重量
        /// </summary>
        [Column("DETAIL_IN_QUANTITY")]
        public decimal DetailInQuantity { set; get; }

        /// <summary>
        /// 餘切重量
        /// </summary>
        [Column("COTANGENT_QUANTITY")]
        public decimal CotangentQuantity { set; get; }

        /// <summary>
        /// 產出重量
        /// </summary>
        [Column("DETAIL_OUT_QUANTITY")]
        public decimal DetailOutQuantity { set; get; }

        /// <summary>
        /// 損耗重量
        /// </summary>
        [Column("LOSS_WEIGHT")]
        public decimal LossWeight { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        [StringLength(03)]
        [Column("PRIMARY_UOM")]
        public string PrimaryUom { set; get; }


        /// <summary>
        /// 得綠
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
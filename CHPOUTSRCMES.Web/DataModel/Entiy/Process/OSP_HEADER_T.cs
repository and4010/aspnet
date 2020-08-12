using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Process
{
    public class OSP_HEADER_T
    {

        /// <summary>
        /// 加工檔頭ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OSP_HEADER_ID")]
        public long OspHeaderId { set; get; }

        /// <summary>
        /// 文件ID(ops工單ID)
        /// </summary>
        /// 
        [Required]
        [Column("PE_BATCH_ID")]
        public long PeBatchId { set; get; }

        /// <summary>
        /// 文件(工單號碼) 
        /// </summary>
        /// 
        [StringLength(32)]
        [Required]
        [Column("BATCH_NO")]
        public string BatchNo { set; get; }

        /// <summary>
        /// 工單類別(OSP：加工、TMP：代紙)
        /// </summary>
        /// 
        [Required]
        [StringLength(03)]
        [Column("BATCH_TYPE")]
        public string BatchType { set; get; }

        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        [Required]
        [Column("BATCH_STATUS")]
        public long BatchStatus { set; get; }


        /// <summary>
        /// 狀態說明
        /// </summary>
        /// 
        [StringLength(80)]
        [Required]
        [Column("BATCH_STATUS_DESC")]
        public string BatchStatusDesc { set; get; }

        /// <summary>
        /// 作業單元ID(OU)
        /// </summary>
        /// 
        [Required]
        [Column("ORG_ID")]
        public long OrgId { set; get; }


        /// <summary>
        /// 作業單元(OU)
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORG_NAME")]
        public string OrgName { set; get; }

        /// <summary>
        /// 組織ID
        /// </summary>
        /// 
        [Required]
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { set; get; }


        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("ORGANIZATION_CODE")]
        public string OrganizationCode { set; get; }

        /// <summary>
        /// 需求完工日期
        /// </summary>
        [Required]
        [Column("DUE_DATE")]
        public DateTime DueDate { set; get; }

        /// <summary>
        /// 計劃開工日期
        /// </summary>
        [Required]
        [Column("PLAN_START_DATE")]
        public DateTime PlanStartDate { set; get; }

        /// <summary>
        /// 計劃完工日期
        /// </summary>
        [Required]
        [Column("PLAN_CMPLT_DATE")]
        public DateTime PlanCmpltDate { set; get; }


        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [Required]
        [Column("PE_CREATED_BY")]
        public long PeCreatedBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Required]
        [Column("PE_CREATION_DATE")]
        public DateTime PeCreationDate { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Column("PE_LAST_UPDATE_BY")]
        [Required]
        public long PeLastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [Column("PE_LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime PeLastUpdateDate { set; get; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Column("STATUS")]
        [Required]
        public string Stauts { set; get; }


        /// <summary>
        /// 裁切日起(起)
        /// </summary>
        /// 
        [Column("CUTTING_DATE_FROM")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime CuttingDateFrom { set; get; }

        /// <summary>
        /// 裁切日起(訖)
        /// </summary>
        /// 
        [Column("CUTTING_DATE_TO")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime CuttingDateTo { set; get; }


        /// <summary>
        /// 機台
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("MACHINE_CODE")]
        public string MachineCode{ set; get; }


        /// <summary>
        /// 生產備註
        /// </summary>
        /// 
        [StringLength(500)]
        [Column("NOTE")]
        public string Note { set; get; }

        /// <summary>
        /// 修改次數
        /// </summary>
        /// 
        [Column("MODIFICATIONS")]
        public int Modifications { set; get; }
    }
}
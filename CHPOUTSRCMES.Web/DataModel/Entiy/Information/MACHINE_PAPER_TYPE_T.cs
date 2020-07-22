using CHPOUTSRCMES.Web.DataModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Information
{
    [Table("MACHINE_PAPER_TYPE_T")]
    public class MACHINE_PAPER_TYPE_T
    {
        /// <summary>
        /// 庫存組織ID
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
        /// 機台紙別代碼
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(30)]
        [Required]
        [Key]
        [Column("MACHINE_CODE")]
        public string MachineCode { set; get; }

        /// <summary>
        /// 機台紙別意義
        /// </summary>
        /// 
        [StringLength(80)]
        [Required]
        [Column("MACHINE_MEANING")]
        public string MachineMeaning { set; get; }

        /// <summary>
        /// 機台紙別摘要
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("DESCRIPTION")]
        public string Description { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(4)]
        [Required]
        [Column("PAPER_TYPE")]
        public string PaperType { set; get; }

        /// <summary>
        /// 機台
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("MACHINE_NUM")]
        public string MachineNum { set; get; }

        /// <summary>
        /// 供應商編號
        /// </summary>
        /// 
        [StringLength(4)]
        [Required]
        [Column("SUPPLIER_NUM")]
        public string SupplierNum { set; get; }

        /// <summary>
        /// 供應商名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("SUPPLIER_NAME")]
        public string SupplierName { set; get; }


        /// <summary>
        /// 控制欄位  D:刪除
        /// </summary>
        /// 
        [StringLength(1)]
        [Required(AllowEmptyStrings = true)]
        [Column("CONTROL_FLAG", TypeName = "char")]
        public string ControlFlag { set; get; }

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
        [Required]
        public DateTime LastUpdateDate { set; get; }
    }
}
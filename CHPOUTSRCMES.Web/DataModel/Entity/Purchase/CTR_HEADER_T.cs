using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Purchase
{
    public class CTR_HEADER_T
    {

        /// <summary>
        /// 檔頭ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("CTR_HEADER_ID")]
        public long CtrHeaderId { set; get; }



        /// <summary>
        /// 櫃表維護 Header ID
        /// </summary>
        /// 
        [Required]
        [Column("HEADER_ID")]
        public long HeaderId { set; get; }


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
        /// 櫃表維護 Line ID
        /// </summary>
        /// 
        [Required]
        [Column("LINE_ID")]
        public long LineId { set; get; }


        /// <summary>
        /// 櫃號
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("CONTAINER_NO")]
        public string ContainerNo { set; get; }

        /// <summary>
        /// 拖櫃日期時間
        /// </summary>
        /// 
        [Required]
        [Column("MV_CONTAINER_DATE")]
        public DateTime MvContainerDate { set; get; }


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
        /// 倉庫
        /// </summary>
        /// 
        [StringLength(20)]
        [Column("SUBINVENTORY")]
        public string Subinventory { set; get; }


        /// <summary>
        /// 狀態
        /// </summary>
        /// 
        [Required]
        [Column("STATUS")]
        public long Status { set; get; }

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
        /// 更新人員名稱
        /// </summary>
        /// 
        [Column("LAST_UPDATE_USER_NAME")]
        public string LastUpdateUserName { set; get; }

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
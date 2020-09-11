using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Transfer
{

    [Table("TRF_REASON_HEADER_T")]
    public class TRF_REASON_HEADER_T
    {
        /// <summary>
        /// 庫存移轉貨故檔頭ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("TRANSFER_REASON_HEADER_ID")]
        public long TransferReasonHeaderId { set; get; }


        /// <summary>
        /// 作業單元ID(OU)
        /// </summary>
        /// 
        [Required]
        [Column("ORG_ID")]
        public long OrgId { set; get; }


        ///// <summary>
        ///// 作業單元(OU)
        ///// </summary>
        ///// 
        //[StringLength(240)]
        //[Required(AllowEmptyStrings = true)]
        //[Column("ORG_NAME")]
        //public string OrgName { set; get; }

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
        /// 出貨編號 Guid
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("SHIPMENT_NUMBER")]
        public string ShipmentNumber { set; get; }

        /// <summary>
        /// 出庫倉庫
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 出貨儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }

        /// <summary>
        /// 出貨儲位
        /// </summary>
        /// 
        [StringLength(163)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

        /// <summary>
        /// 儲位第三節段
        /// </summary>
        /// 
        [StringLength(40)]
        [Column("SEGMENT3")]
        public string Segment3 { set; get; }

        /// <summary>
        /// 出貨編號狀態
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("NUMBER_STATUS")]
        public string NumberStatus { set; get; }

        /// <summary>
        /// 交易日期
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Required]
        [Column("TRANSACTION_DATE")]
        public DateTime TransactionDate { set; get; }

        /// <summary>
        /// 異動型態ID
        /// </summary>
        /// 
        [Column("TRANSACTION_TYPE_ID")]
        [Required]
        public long TransactionTypeId { set; get; }

        /// <summary>
        /// 異動型態
        /// </summary>
        /// 
        [StringLength(80)]
        [Required]
        [Column("TRANSACTION_TYPE_NAME")]
        public string TransactionTypeName { set; get; }

        /// <summary>
        /// 目標作業單元ID(OU)
        /// </summary>
        /// 
        [Column("TRANSFER_ORG_ID")]
        public long? TransferOrgId { set; get; }

        /// <summary>
        /// 目標作業單元(OU)
        /// </summary>
        /// 
        //[StringLength(240)]
        //[Column("TRANSFER_ORG_NAME")]
        //public string TransferOrgName { set; get; }

        /// <summary>
        /// 目標庫存組織ID
        /// </summary>
        /// 
        [Column("TRANSFER_ORGANIZATION_ID")]
        public long? TransferOrganizationId { set; get; }

        /// <summary>
        /// 目標庫存組織
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("TRANSFER_ORGANIZATION_CODE")]
        public string TransferOrganizationCode { set; get; }

        /// <summary>
        /// 目標倉庫
        /// </summary>
        /// 
        [StringLength(20)]
        [Column("TRANSFER_SUBINVENTORY_CODE")]
        public string TransferSubinventoryCode { set; get; }

        /// <summary>
        /// 目標儲位ID
        /// </summary>
        /// 
        [Column("TRANSFER_LOCATOR_ID")]
        public long? TransferLocatorId { set; get; }

        /// <summary>
        /// 目標儲位
        /// </summary>
        /// 
        [StringLength(163)]
        [Column("TRANSFER_LOCATOR_CODE")]
        public string TransferLocatorCode { set; get; }

        
        /// <summary>
        /// 是否傳給ERP
        /// </summary>
        [StringLength(10)]
        [Column("TO_ERP")]
        public string ToErp { set; get; }

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
        [StringLength(128)]
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
        /// 更新人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_BY")]
        public string LastUpdateBy { set; get; }

        /// <summary>
        /// 更新人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("LAST_UPDATE_USER_NAME")]
        public string LastUpdateUserName { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [Column("LAST_UPDATE_DATE")]
        public DateTime? LastUpdateDate { set; get; }


    }
}
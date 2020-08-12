using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Delivery
{
    [Table("DLV_HEADER_T")]
    public class DLV_HEADER_T
    {
        /// <summary>
        /// 出庫檔頭ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("DLV_HEADER_ID")]
        public long DlvHeaderId { set; get; }

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
        [Required(AllowEmptyStrings=true)]
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
        /// 出貨倉庫
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("SUBINVENTORY_CODE")]
        public string SubinventoryCode { set; get; }

        /// <summary>
        /// 車次
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("TRIP_CAR")]
        public string TripCar { set; get; }

        /// <summary>
        /// 航程號ID
        /// </summary>
        /// 
        [Required]
        [Column("TRIP_ID")]
        public long TripId { set; get; }

        /// <summary>
        /// 航程號
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("TRIP_NAME")]
        public string TripName { set; get; }

        /// <summary>
        /// 組車日
        /// </summary>
        /// 
        [Column("TRIP_ACTUAL_SHIP_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime TripActualShipDate { set; get; }

        /// <summary>
        /// 交貨單ID
        /// </summary>
        /// 
        [Required]
        [Column("DELIVERY_ID")]
        public long DeliveryId { set; get; }

        /// <summary>
        /// 交運單號
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("DELIVERY_NAME")]
        public string DeliveryName { set; get; }

        /// <summary>
        /// 捲筒\平板
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("ITEM_CATEGORY")]
        public string ItemCategory { set; get; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        /// 
        [Required]
        [Column("CUSTOMER_ID")]
        public long CustomerId { set; get; }

        /// <summary>
        /// 訂單客戶編號
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("CUSTOMER_NUMBER")]
        public string CustomerNumber { set; get; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        /// 
        [StringLength(500)]
        [Required]
        [Column("CUSTOMER_NAME")]
        public string CustomerName { set; get; }

        /// <summary>
        /// 送貨地點
        /// </summary>
        /// 
        [StringLength(500)]
        [Required]
        [Column("CUSTOMER_LOCATION_CODE")]
        public string CustomerLocationCode { set; get; }

        /// <summary>
        /// 送貨客戶ID
        /// </summary>
        /// 
        [Required]
        [Column("SHIP_CUSTOMER_ID")]
        public long ShipCustomerId { set; get; }

        /// <summary>
        /// 送貨客戶編號
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("SHIP_CUSTOMER_NUMBER")]
        public string ShipCustomerNumber { set; get; }

        /// <summary>
        /// 送貨客戶名稱
        /// </summary>
        /// 
        [StringLength(500)]
        [Required]
        [Column("SHIP_CUSTOMER_NAME")]
        public string ShipCustomerName { set; get; }

        /// <summary>
        /// 送貨客戶地點
        /// </summary>
        /// 
        [StringLength(500)]
        [Required]
        [Column("SHIP_LOCATION_CODE")]
        public string ShipLocationCode { set; get; }

        /// <summary>
        /// 內銷地區別
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("FREIGHT_TERMS_NAME")]
        public string FreightTermsName { set; get; }

        /// <summary>
        /// 交貨單狀態
        /// </summary>
        /// 
        [StringLength(10)]
        [Required]
        [Column("DELIVERY_STATUS_CODE")]
        public string DeliveryStatusCode { set; get; }

        /// <summary>
        /// 交貨單狀態名稱
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("DELIVERY_STATUS_NAME")]
        public string DeliveryStatusName { set; get; }

        /// <summary>
        /// 出貨確認人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("TRANSACTION_BY")]
        public string TransactionBy { set; get; }

        /// <summary>
        /// 出貨確認人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("TRANSACTION_USER_NAME")]
        public string TransactionByUserNmae { set; get; }

        /// <summary>
        /// 出貨確認日期
        /// </summary>
        /// 
        [Column("TRANSACTION_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { set; get; }


        /// <summary>
        /// 出貨核准人員
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("AUTHORIZE_BY")]
        public string AuthorizeBy { set; get; }

        /// <summary>
        /// 出貨核准人員名稱
        /// </summary>
        /// 
        [StringLength(128)]
        [Column("AUTHORIZE_USER_NAME")]
        public string AuthorizeByUserName { set; get; }

        /// <summary>
        /// 出貨核准日期
        /// </summary>
        /// 
        [Column("AUTHORIZE_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AuthorizeDate { set; get; }

        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("NOTE")]
        public string Note { set; get; }

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
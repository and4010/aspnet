using CHPOUTSRCMES.DataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Delivery
{
    [Table("DLV_ORG_T")]
    public class DLV_ORG_T
    {
        /// <summary>
        /// 出庫主檔ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("DLV_ORG_ID")]
        public long DlvOrgId { set; get; }
        	
        /// <summary>
        /// XXIFP220
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("PROCESS_CODE")]
        public string ProcessCode { set; get; }


        /// <summary>
        /// 
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("SERVER_CODE")]
        public string ServerCode { set; get; }


        /// <summary>
        /// 20191112141600100000
        /// </summary>
        /// 
        [StringLength(20)]
        [Required]
        [Column("BATCH_ID")]
        public string BatchId { set; get; }


        /// <summary>
        /// 1
        /// </summary>
        /// 
        [Required]
        [Column("BATCH_LINE_ID")]
        public long BatchLineId { set; get; }

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
        public string Freight_Terms_Name { set; get; }

        /// <summary>
        /// 訂單ID
        /// </summary>
        /// 
        [Required]
        [Column("ORDER_HEADER_ID")]
        public long OrderHeaderId { set; get; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        /// 
        [Required]
        [Column("Order_Number")]
        public long ORDER_NUMBER { set; get; }

        /// <summary>
        /// 訂單明細ID
        /// </summary>
        /// 
        [Required]
        [Column("ORDER_LINE_ID")]
        public long OrderLineId { set; get; }

        /// <summary>
        /// 訂單行號
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ORDER_SHIP_NUMBER")]
        public string OrderShipNumber { set; get; }

        /// <summary>
        /// 出貨明細ID
        /// </summary>
        /// 
        [Required]
        [Column("DELIVERY_DETAIL_ID")]
        public long DeliveryDetailId { set; get; }

         /// <summary>
        /// 包裝方式
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("PACKING_TYPE")]
        public string Packing_Type { set; get; }

        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        [Required]
        [Column("INVENTORY_ITEM_ID")]
        public long Inventory_Item_Id { set; get; }

        /// <summary>
        /// 料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("ITEM_NUMBER")]
        public string Item_Number { set; get; }

        /// <summary>
        /// 料號名稱
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESCRIPTION")]
        public string Item_Description { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("PAPER_TYPE")]
        public string PaperType { set; get; }

        /// <summary>
        /// 基重
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("BASIC_WEIGHT")]
        public string BasicWeight { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("SPECIFICATION")]
        public string Specification { set; get; }

        /// <summary>
        /// 絲向
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("GRAIN_DIRECTION")]
        public string GrainDirection { set; get; }

        /// <summary>
        /// 出貨倉庫
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
        [StringLength(30)]
        [Column("LOCATOR_CODE")]
        public string LOCATOR_CODE { set; get; }

        /// <summary>
        /// 訂單原始交易數量
        /// </summary>
        /// 
        [Required]
        [Column("SRC_REQUESTED_QUANTITY")]
        [Precision(30, 10)]
        public decimal SrcRequestedQuantity { set; get; }

        /// <summary>
        /// 訂單交易單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("SRC_REQUESTED_QUANTITY_UOM")]
        public string SrcRequestedQuantityUom { set; get; }

        /// <summary>
        /// 預計出庫主要數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_QUANTITY")]
        [Precision(30, 10)]
        public decimal RequestedQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_QUANTITY_UOM")]
        public string RequestedQuantityUom { set; get; }

        /// <summary>
        /// 預計出庫次要數量
        /// </summary>
        /// 
        [Required]
        [Column("REQUESTED_QUANTITY2")]
        [Precision(30, 10)]
        public decimal RequestedQuantity2 { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [Required]
        [StringLength(3)]
        [Column("REQUESTED_QUANTITY_UOM2")]
        public string RequestedQuantityUom2 { set; get; }
    
        ///// <summary>
        ///// 批號
        ///// </summary>
        ///// 
        //[StringLength(80)]
        //[Column("LOT_NUMBER")]
        //public string? Lot_Number { set; get; }
    
        ///// <summary>
        ///// 批號數量
        ///// </summary>
        ///// 
        //[Column("LOT_QUANTITY")]
        //public long? LotQuantity { set; get; }


        /// <summary>
        /// 備註
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("NOTE")]
        public string Note { set; get; }

        /// <summary>
        /// ERP請求識別碼
        /// </summary>
        /// 
        [Required]
        [Column("REQUEST_ID")]
        public long? RequestId { set; get; }

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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime LastUpdateDate { set; get; }
    }
}
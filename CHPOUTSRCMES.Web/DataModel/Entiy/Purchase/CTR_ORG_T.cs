using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Purchase
{
    public class CTR_ORG_T
    {

        /// <summary>
        /// 入庫ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Column("CTR_ORG_ID")]
        public long CtrOrgId { set; get; }


        /// <summary>
        /// XXIFP217
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
        [Required]
        [Column("SUBINVENTORY")]
        public string Subinventory { set; get; }


        /// <summary>
        /// 儲位ID
        /// </summary>
        /// 
        [Column("LOCATOR_ID")]
        public long? LocatorId { set; get; }


        /// <summary>
        /// 儲位
        /// </summary>
        /// 
        [StringLength(30)]
        [Column("LOCATOR_CODE")]
        public string LocatorCode { set; get; }

        /// <summary>
        /// 櫃表維護 Detail ID
        /// </summary>
        /// 
        [Required]
        [Column("DETAIL_ID")]
        public long DetailId { set; get; }

        /// <summary>
        /// 料號ID
        /// </summary>
        /// 
        [Required]
        [Column("INVENTORY_ITEM_ID")]
        public long InventoryItemId { set; get; }


        /// <summary>
        /// 料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("SHIP_ITEM_NUMBER")]
        public string ShipItemNumber { set; get; }

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
        /// 令重
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("REAM_WEIGHT")]
        public string ReamWeight { set; get; }

        /// <summary>
        /// 捲數\棧板數
        /// </summary>
        /// 
        [Required]
        [Column("ROLL_REAM_QTY")]
        public decimal RollReamQty { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 
        [Required]
        [Column("ROLL_REAM_WT")]
        public decimal RollReamWt { set; get; }

        /// <summary>
        /// 總捲數\令數
        /// </summary>
        /// 
        [Required]
        [Column("TTL_ROLL_REAM")]
        public decimal TtlRollReam { set; get; }

        /// <summary>
        /// 規格
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("SPECIFICATION")]
        public string Specification { set; get; }

        /// <summary>
        /// 令包\無令打件
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("PACKING_TYPE")]
        public string PackingType { set; get; }

        /// <summary>
        /// 出貨數量(MT)
        /// </summary>
        /// 
        [Required]
        [Column("SHIP_MT_QTY")]
        public decimal ShipMtQty { set; get; }

        /// <summary>
        /// 交易數量
        /// </summary>
        /// 
        [Required]
        [Column("TRANSACTION_QUANTITY")]
        public decimal TransactionQuantity { set; get; }

        /// <summary>
        /// 交易單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("TRANSACTION_UOM")]
        public string TransactionUom { set; get; }

        /// <summary>
        /// 主要數量
        /// </summary>
        /// 
        [Required]
        [Column("PRIMARY_QUANTITY")]
        public decimal PrimaryQuantity { set; get; }

        /// <summary>
        /// 主要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("PRIMARY_UOM")]
        public string PrimaryUom { set; get; }

        /// <summary>
        /// 次要數量
        /// </summary>
        /// 
        [Column("SECONDARY_QUANTITY")]
        public decimal? SecondaryQuantity { set; get; }

        /// <summary>
        /// 次要單位
        /// </summary>
        /// 
        [StringLength(3)]
        [Column("SECONDARY_UOM")]
        public string SecondaryUom { set; get; }


        /// <summary>
        /// 批號
        /// </summary>
        /// 
        [StringLength(80)]
        [Required(AllowEmptyStrings = true)]
        [Column("LOT_NUMBER")]
        public string LotNumber { set; get; }

        /// <summary>
        /// 理論重(批號數量)
        /// </summary>
        /// 
        [StringLength(30)]
        [Required(AllowEmptyStrings = true)]
        [Column("THEORY_WEIGHT")]
        public string TheoryWeight { set; get; }


        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE1")]
        public string Attribute1 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE2")]
        public string Attribute2 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE3")]
        public string Attribute3 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE4")]
        public string Attribute4 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE5")]
        public string Attribute5 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE6")]
        public string Attribute6 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE7")]
        public string Attribute7 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE8")]
        public string Attribute8 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE9")]
        public string Attribute9 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE10")]
        public string Attribute10 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE11")]
        public string Attribute11 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE12")]
        public string Attribute12 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]

        [Column("ATTRIBUTE13")]
        public string Attribute13 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE14")]
        public string Attribute14 { set; get; }

        /// <summary>
        /// 預留欄位
        /// </summary>
        /// 
        [StringLength(240)]
        [Column("ATTRIBUTE15")]
        public string Attribute15 { set; get; }

        /// <summary>
        /// ERP請求識別碼
        /// </summary>
        /// 
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
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime LastUpdateDate { set; get; }
    }
}
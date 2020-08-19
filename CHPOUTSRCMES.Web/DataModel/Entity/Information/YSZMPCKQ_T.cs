using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Information
{
    [Table("YSZMPCKQ_T")]
    public class YSZMPCKQ_T
    {
        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("YSZMPCKQ_ID")]
        public long Yszmpckq_ID { set; get; }



        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        [Required]
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { set; get; }


        /// <summary>
        /// 庫存組織CODE
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("ORGANIZATION_CODE")]
        public string OrganizationCode { set; get; }


        /// <summary>
        /// 加工廠
        /// </summary>
        /// 
        [StringLength(10)]
        [Column("OSP_SUBINVENTORY")]
        public string OspSubinventory { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(4)]
        [Required]
        [Column("PSTYP")]
        public string Pstyp { set; get; }


        /// <summary>
        /// 基重上限
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [Column("BWETUP")]
        public decimal Bwetup { set; get; }

        /// <summary>
        /// 基重下限
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [Column("BWETDN")]
        public decimal Bwetdn { set; get; }

        /// <summary>
        /// 令重上限
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [Column("RWTUP")]
        public decimal Rwtup { set; get; }

        /// <summary>
        /// 令重下限
        /// </summary>
        /// 
        [Required(AllowEmptyStrings = true)]
        [Column("RWTDN")]
        public decimal Rwtdn { set; get; }

        /// <summary>
        /// 包數
        /// </summary>
        /// 

        [Required(AllowEmptyStrings = true)]
        [Column("PCKQ")]
        public long Pckq { set; get; }

        /// <summary>
        /// 每包張數
        /// </summary>
        /// 

        [Required(AllowEmptyStrings = true)]
        [Column("PAPER_QTY")]
        public long PaperQty { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 

        [Required(AllowEmptyStrings = true)]
        [Column("PIECES_QTY")]
        public long PiecesQty { set; get; }

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
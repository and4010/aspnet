using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("YSZMPCKQ_T")]
    public class YSZMPCKQ_T
    {

        /// <summary>
        /// 庫存組織
        /// </summary>
        /// 
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
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
        [Required]
        [Column("OSP_SUBINVENTORY")]
        public string OspSubinventory { set; get; }

        /// <summary>
        /// 紙別
        /// </summary>
        /// 
        [StringLength(3)]
        [Required]
        [Column("PSTYP")]
        public string Pstyp { set; get; }


        /// <summary>
        /// 基重上限
        /// </summary>
        /// 
        [Required]
        [Column("BWETUP")]
        public long Bwetup { set; get; }

        /// <summary>
        /// 基重下限
        /// </summary>
        /// 
        [Required]
        [Column("BWETDN")]
        public long Bwetdn { set; get; }

        /// <summary>
        /// 令重上限
        /// </summary>
        /// 
        [Required]
        [Column("RWTUP")]
        public long Rwtup { set; get; }

        /// <summary>
        /// 令重下限
        /// </summary>
        /// 
        [Required]
        [Column("RWTDN")]
        public long Rwtdn { set; get; }

        /// <summary>
        /// 包數
        /// </summary>
        /// 

        [Required]
        [Column("PCKQ")]
        public long Pckq { set; get; }

        /// <summary>
        /// 每包張數
        /// </summary>
        /// 

        [Required]
        [Column("PAPER_QTY")]
        public long PaperQty { set; get; }

        /// <summary>
        /// 每件令數
        /// </summary>
        /// 

        [Required]
        [Column("PIECES_QTY")]
        public long PiecesQty { set; get; }


        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [Column("CREATE_BY")]
        [Required]
        public long CreateBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [Column("CREATE_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime CreateDate { set; get; }

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
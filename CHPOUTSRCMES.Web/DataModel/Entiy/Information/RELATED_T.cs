using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Information
{
    [Table("RELATED_T")]
    public class RELATED_T
    {
        /// <summary>
        /// 餘切規格ID
        /// </summary>
        /// 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [Column("RELATED_ID")]
        public long RelatedId { set; get; }

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
        [Column("ITEM_NUMBER")]
        public string ItemNumber { set; get; }

        /// <summary>
        /// 料號說明
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("ITEM_DESCRIPTION")]
        public string ItemDescription { set; get; }

        /// <summary>
        /// 餘切料號ID
        /// </summary>
        /// 
        [Required]
        [Column("RELATED_ITEM_ID")]
        public long RelatedItemId { set; get; }

        /// <summary>
        /// 餘切料號
        /// </summary>
        /// 
        [StringLength(40)]
        [Required]
        [Column("RELATED_ITEM_NUMBER")]
        public string RelatedItemNumber { set; get; }

        /// <summary>
        /// 餘切料號說明
        /// </summary>
        /// 
        [StringLength(240)]
        [Required]
        [Column("RELATED_ITEM_DESCRIPTION")]
        public string RelatedItemDescription { set; get; }


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
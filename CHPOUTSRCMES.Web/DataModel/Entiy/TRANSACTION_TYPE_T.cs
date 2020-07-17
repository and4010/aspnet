using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    public class TRANSACTION_TYPE_T
    {

        /// <summary>
        /// 異動型態ID
        /// </summary>
        /// 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("TRANSACTION_TYPE_ID")]
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
        /// 異動型態摘要
        /// </summary>
        /// 
        [StringLength(2400)]
        [Required]
        [Column("DESCRIPTION")]
        public string Description { set; get; }


        /// <summary>
        /// 異動作業id
        /// </summary>
        /// 
        [Required]
        [Column("TRANSACTION_ACTION_ID")]
        public long TransactionActionId { set; get; }


        /// <summary>
        /// 異動作業
        /// </summary>
        /// 
        [StringLength(80)]
        [Required]
        [Column("TRANSACTION_ACTION_NAME")]
        public string TransactionActionName { set; get; }


        /// <summary>
        /// 來源型態ID
        /// </summary>
        /// 
        [Required]
        [Column("TRANSACTION_SOURCE_TYPE_ID")]
        public long TransactionSourceTypeId { set; get; }


        /// <summary>
        /// 來源型態
        /// </summary>
        /// 
        [StringLength(30)]
        [Required]
        [Column("TRANSACTION_SOURCE_TYPE_NAME")]
        public string TransactionSourceTypeName { set; get; }


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
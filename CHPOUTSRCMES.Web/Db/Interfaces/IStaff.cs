using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Interfaces
{
    public interface IStaff
    {

        /// <summary>
        /// 建立人員
        /// </summary>
        /// 
        [Column(TypeName = "CREATE_BY")]
        [Required]
        int CreateBy { set; get; }

        /// <summary>
        /// 建立時間
        /// </summary>
        /// 
        [Column(TypeName = "CREATE_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        DateTime CreateDate { set; get; }

        /// <summary>
        /// 更新人員
        /// </summary>
        /// 
        [Column(TypeName = "LAST_UPDATE_BY")]
        [Required]
        int LastUpdateBy { set; get; }

        /// <summary>
        /// 更新時間
        /// </summary>
        /// 
        [Column(TypeName = "LAST_UPDATE_DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        DateTime LastUpdateDate { set; get; }
    }
}
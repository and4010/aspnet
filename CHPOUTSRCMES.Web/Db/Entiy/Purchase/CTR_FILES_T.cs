﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Db.Entiy.Purchase
{
    public class CTR_FILES_T
    {

        /// <summary>
        /// 檔案ID
        /// </summary>
        /// 
        [Key]
        [Required]
        [Column("CTR_FILE_ID")]
        public long CtrFileId { set; get; }


        /// <summary>
        /// 檔案實體
        /// </summary>
        /// 
        [Column("FILE_INSTANCE")]
        [Required]
        public Byte[] FileInstance { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("LOG_ENTRY_T")]
    public class LOG_ENTRY_T
    {
        public int Id { get; set; }
        [StringLength(5000)]
        public string CallSite { get; set; }
         [StringLength(30)]
        public string Date { get; set; }
        [StringLength(5000)]
        public string Exception { get; set; }
        [StringLength(30)]
        public string Level { get; set; }
        [StringLength(100)]
        public string Logger { get; set; }
        [StringLength(300)]
        public string MachineName { get; set; }
        [StringLength(5000)]
        public string Message { get; set; }
        [StringLength(5000)]
        public string StackTrace { get; set; }
        [StringLength(300)]
        public string Thread { get; set; }
        [StringLength(300)]
        public string Username { get; set; }
    }
}
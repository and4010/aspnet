using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHPOUTSRCMES.Web.Util
{
    public class ConvertDateTime
    {
        public static string ConverYYYY(DateTime dateTime)
        {
          return  dateTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
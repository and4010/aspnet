using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CHPOUTSRCMES.Web.Util
{
    public class LogUtilities
    {
        /// <summary>

        /// Builds the exception message.

        /// </summary>

        /// <param name="x">The x.</param>

        /// <returns></returns>

        public static string BuildHttpExceptionMessage(Exception x)
        {
            Exception logException = x;

            if (x.InnerException != null)
            {
                logException = x.InnerException;
            }

            StringBuilder message = new StringBuilder();

            message.AppendLine();
            message.AppendLine(" Error in Path : " + System.Web.HttpContext.Current.Request.Path);

            // Get the QueryString along with the Virtual Path
            message.AppendLine("Raw Url : " + System.Web.HttpContext.Current.Request.RawUrl);

            // Type of Exception
            message.AppendLine("Type of Exception : " + logException.GetType().Name);

            // Get the error message
            message.AppendLine("Message : " + logException.Message);

            // Source of the message
            message.AppendLine("Source : " + logException.Source);

            // Stack Trace of the error
            message.AppendLine("Stack Trace : " + logException.StackTrace);

            // Method where the error occurred
            message.AppendLine("TargetSite : " + logException.TargetSite);

            return message.ToString();

        }

        public static string BuildExceptionMessage(Exception x)
        {
            Exception logException = x;

            if (x.InnerException != null)
            {
                logException = x.InnerException;
            }

            StringBuilder message = new StringBuilder();

            // Type of Exception
            message.AppendLine("Type of Exception : " + logException.GetType().Name);

            // Get the error message
            message.AppendLine("Message : " + logException.Message);

            // Stack Trace of the error
            message.AppendLine("Stack Trace : " + logException.StackTrace);

            return message.ToString();

        }

        //public static string BuildResponseMessage(Response response)
        //{
        //    StringBuilder logBuilder = new StringBuilder();
        //    logBuilder.AppendLine(" Response:");
        //    logBuilder.AppendLine("Code:" + response.CODE);
        //    logBuilder.AppendLine("Msg:" + response.MSG);
        //    if (response.CONTENT != null)
        //    {
        //        logBuilder.AppendLine("Content:" + response.CONTENT);
        //    }
        //    return logBuilder.ToString();
        //}

        //public static string BuildResultMessage(IResult result)
        //{
        //    StringBuilder logBuilder = new StringBuilder();
        //    logBuilder.AppendLine(" Result:");
        //    logBuilder.AppendLine(" IsSuccess:" + result.Success);
        //    logBuilder.AppendLine(" Message:" + result.Message);
        //    if (result.Exception != null)
        //    {
        //        logBuilder.Append(" Exception:" + BuildHttpExceptionMessage(result.Exception));
        //    }
        //    return logBuilder.ToString();
        //}
    }
}

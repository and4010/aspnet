using CHPOUTSRCMES.Web.Models;
using CHPOUTSRCMES.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult General(Exception exception)
        {
            string msg = "";
            int code = 0;
            string StackTrace = "";
            if (exception != null)
            {
                switch (exception.GetType().ToString())
                {
                    case "System.Web.HttpException":
                        System.Web.HttpException httpEx = (System.Web.HttpException)exception;
                        switch (httpEx.GetHttpCode())
                        {
                            case 404:
                                msg = httpEx.GetHttpCode() + "找不到網頁";
                                code = httpEx.ErrorCode;
                                StackTrace = httpEx.StackTrace;
                                break;
                            default:
                                msg = httpEx.GetHttpCode() + httpEx.Message;
                                code = httpEx.ErrorCode;
                                StackTrace = httpEx.StackTrace;
                                break;
                        }
                        break;
                    default:
                        msg = $"exception {exception.GetType().ToString()} \n msg : {exception.Message}";
                        StackTrace = exception.StackTrace;
                        break;
                }
            }
            var model = new ErrorViewModel()
            {
                Error = new Error()
                {
                    Id = 1,
                    Message = msg,
                    Code = code,
                    StackTrace = StackTrace
                }
            };
            return View(model);
        }
        
      

       

	}
}
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
            if (exception != null)
            {
                switch (exception.GetType().ToString())
                {
                    case "System.Web.HttpException":
                        System.Web.HttpException httpEx = (System.Web.HttpException)exception;
                        switch (httpEx.GetHttpCode())
                        {
                            case 404:
                                msg = "找不到網頁";
                                break;
                            default:
                                msg = exception.Message;
                                break;
                        }
                        break;
                    default:
                        msg = exception.Message;
                        break;
                }
            }
            var model = new ErrorViewModel()
            {
                Error = new Error()
                {
                    Id = 1,
                    Message = msg
                }
            };
            return View(model);
        }
        
      

       

	}
}
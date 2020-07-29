using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHPOUTSRCMES.Web.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        //
        // GET: /Transfer/
        public ActionResult Index()
        {
            return View();
        }
	}
}
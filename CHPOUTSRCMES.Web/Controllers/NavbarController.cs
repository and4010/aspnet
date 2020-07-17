using System.Linq;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.Domain;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class NavbarController : Controller
    {
        private MesContext db = new MesContext();
        // GET: Navbar
        public ActionResult Index()
        { 
            var data = new Domain.Data();
            db.LocatorTs.ToList();
            return PartialView("_Navbar", data.navbarItems().ToList());
        }
    }
}
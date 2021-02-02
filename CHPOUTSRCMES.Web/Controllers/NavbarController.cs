using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.Domain;

namespace CHPOUTSRCMES.Web.Controllers
{
    public class NavbarController : Controller
    {
        private MesContext db = new MesContext();

        /// <summary>
        /// 左側導覽Menu View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //取得使用者角色
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList()[0].Value;

            var data = new Domain.Data();
            
            return PartialView("_Navbar", data.navbarItems(roles).ToList());
        }
    }
}
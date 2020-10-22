using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CHPOUTSRCMES.Web.DataModel.Entity
{
    [Table("USER_T")]
    public class AppUser : IdentityUser
    {
        [StringLength(15)]
        public string DisplayName { set; get; }
        //public string Email { set; get; }
       
        [StringLength(1)]
        public string Flag { set; get; }

        [StringLength(3)]
        public string Status { set; get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
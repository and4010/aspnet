using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CHPOUTSRCMES.Web.DataModel.Entiy
{
    [Table("USER_T")]
    public class AppUser : IdentityUser
    {
        [StringLength(15)]
        public string DisplayName { set; get; }
        //public string Email { set; get; }
        public long OrganizationId { set; get; }
        public string OrganizationCode { set; get; }
        public string SubinventoryCode { set; get; }
        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
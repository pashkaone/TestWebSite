using Microsoft.AspNet.Identity.EntityFramework;
 
namespace FormAuth.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        { }

    public string Description { get; set; }
    }
}
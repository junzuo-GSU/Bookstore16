using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;  // for NotMapped

namespace Bookstore.Models
{
    public class User : IdentityUser
    {
        [NotMapped]
        public IList<string> RoleNames { get; set; } = null!;

        //To add other properties not defined in the base class, simply place them here
        // public String FirstName {get;set;} = String.Empty;
        // public String LastName {get;set;} = String.Empty;
        // public DateTime DOB  {get;set;} = DateTime.MinValue;
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        // public override String Email {get;set;} = String.Empty;
    }
}

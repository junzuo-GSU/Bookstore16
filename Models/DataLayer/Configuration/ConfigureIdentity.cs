using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Models
{
    /*
    ConfigureIdentity is to add the seeding data, admin, as a user for the app.
    */
    public class ConfigureIdentity
    {
        /*
        this method is invoked at application startup from Program.cs. 
        It will be invoked only once as a scoped service. Therefore its accessor is 'static' for the sake of convenience.
        */
        public static async Task CreateAdminUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<User>>();

            string username = "admin";
            string password = "Sesame";
            string roleName = "Admin";
            
            //always creating role before user so that the user can be assigned to a role after creation.
            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // if username doesn't exist, create it and add to role
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    //note the role assignment is handled by UserManager, not RoleManager.
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }

}

using Book.Models;
using Book.Utilites;
using BulkyBook.DataAcess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAceess.DbInitializer
{
    public class DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        AppDbContext context) : IDbInitializer
    {
        public void Initialize()
        {

            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();


                //if roles are not created, then we will create admin user as well
                userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "Mohamed Zonkol",
                    PhoneNumber = "01029054588",
                    StreetAddress = "Cairo",
                    State = "IL",
                    PostalCode = "23422",
                    City = "Zag"
                }, "Mo@123").GetAwaiter().GetResult();
                ApplicationUser user =
                    context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}

using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class ApplicationUserRepository(AppDbContext context) : Repository<ApplicationUser>(context), IApplicationUserRepository
    {
        public void Update(ApplicationUser obj)
        {
            context.ApplicationUsers.Update(obj);
        }
    }
}

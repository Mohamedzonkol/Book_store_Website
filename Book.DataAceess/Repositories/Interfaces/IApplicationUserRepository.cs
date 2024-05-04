using Book.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
}

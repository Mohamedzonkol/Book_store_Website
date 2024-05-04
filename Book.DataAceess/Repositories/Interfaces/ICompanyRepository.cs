using Book.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}

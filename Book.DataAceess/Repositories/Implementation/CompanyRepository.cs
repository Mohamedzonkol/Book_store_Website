using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class CompanyRepository(AppDbContext context) : Repository<Company>(context), ICompanyRepository
    {
        public void Update(Company obj)
        {
            context.Companies.Update(obj);
        }
    }
}

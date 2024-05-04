using Book.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}

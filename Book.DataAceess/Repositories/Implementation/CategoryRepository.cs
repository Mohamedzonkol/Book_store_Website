using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
    {
        public void Update(Category obj)
        {
            context.Categories.Update(obj);
        }
    }
}

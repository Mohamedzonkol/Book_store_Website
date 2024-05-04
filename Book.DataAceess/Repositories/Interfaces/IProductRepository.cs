using BulkyBook.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}

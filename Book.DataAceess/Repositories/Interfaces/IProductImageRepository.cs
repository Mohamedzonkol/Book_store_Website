using BulkyBook.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        void Update(ProductImage obj);
    }
}

using Book.DataAceess.Repositories.Interfaces;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;

namespace Book.DataAceess.Repositories.Implementation
{
    public class ProductImageRepository(AppDbContext context) : Repository<ProductImage>(context), IProductImageRepository
    {
        public void Update(ProductImage obj)
        {
            context.ProductImages.Update(obj);
        }
    }
}

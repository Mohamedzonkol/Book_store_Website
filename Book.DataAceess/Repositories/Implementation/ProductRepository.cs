using Book.DataAceess.Repositories.Interfaces;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;

namespace Book.DataAceess.Repositories.Implementation
{
    public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository

    {
        public void Update(Product obj)
        {
            context.Products.Update(obj);
        }
    }
}

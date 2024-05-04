using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class ShoppingCartRepository(AppDbContext context) : Repository<ShoppingCart>(context), IShoppingCartRepository
    {
        public void Update(ShoppingCart obj)
        {
            context.ShoppingCarts.Update(obj);
        }
    }
}

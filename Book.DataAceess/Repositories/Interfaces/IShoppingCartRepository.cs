using Book.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}

using Book.Models;

namespace Book.DataAceess.Repositories.Interfaces
{
    public interface IOrderDetailsRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}

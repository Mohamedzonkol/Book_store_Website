using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class OrderDetailsRepository(AppDbContext context) : Repository<OrderDetail>(context), IOrderDetailsRepository
    {
        public void Update(OrderDetail obj)
        {
            context.OrderDetails.Update(obj);
        }
    }
}

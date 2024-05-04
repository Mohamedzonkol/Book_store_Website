using Book.DataAceess.Repositories.Interfaces;
using BulkyBook.DataAcess.Data;

namespace Book.DataAceess.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public ICompanyRepository Company { get; }
        public IProductImageRepository ProductImage { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailsRepository OrderDetails { get; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
            Company = new CompanyRepository(_context);
            ProductImage = new ProductImageRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

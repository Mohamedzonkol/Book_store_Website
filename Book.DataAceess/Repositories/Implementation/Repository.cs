using Book.DataAceess.Repositories.Interfaces;
using BulkyBook.DataAcess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Book.DataAceess.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private DbSet<T> dbSet;
        protected Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperty = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var property in includeProperty.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();

        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}

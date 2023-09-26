using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly DbSet<T> _db;

        public GenericRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _db = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = _db.Find(id);
            _db.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public IEnumerable<T> GetAll()
        {
            return _db.ToList();
        }

        public T GetById(int id)
        {
            return _db.Find(id);
        }

        public T GetById(string id)
        {
            return _db.Find(id);
        }

        public void Update(int id, T entity)
        {
            _db.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(string id, T entity)
        {
            _db.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}


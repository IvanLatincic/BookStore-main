namespace BookStore.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetById(string id);
        void Add(T entity);
        void Update(int id, T entity);
        void Update(string id, T entity);
        void Delete(int id);
    }
}

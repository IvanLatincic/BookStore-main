using BookStore.Models;

namespace BookStore.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookStoreDbContext _dbContext;

        public UnitOfWork(IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<BookStoreDbContext>();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        private IGenericRepository<ApplicationUser> _users;
        private IGenericRepository<Category> _categories;
        private ISubCategoryRepository _subCategories;
        private IProductRepository _products;
        private IOrderRepository _orders;
        private IGenericRepository<ShippingInfo> _shippingInfos;
        public IGenericRepository<ApplicationUser> ApplicationUsers => _users ??= new GenericRepository<ApplicationUser>(_dbContext);
        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_dbContext);
        public ISubCategoryRepository SubCategories => _subCategories ??= new SubCategoryRepository(_dbContext);
        public IProductRepository Products => _products ??= new ProductRepository(_dbContext);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_dbContext);
        public IGenericRepository<ShippingInfo> ShippingInfos => _shippingInfos ??= new GenericRepository<ShippingInfo>(_dbContext);
    }
}

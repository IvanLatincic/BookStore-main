using BookStore.Models;

namespace BookStore.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<ApplicationUser> ApplicationUsers { get; }
        IGenericRepository<Category> Categories { get; }
        ISubCategoryRepository SubCategories { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }

        IGenericRepository<ShippingInfo> ShippingInfos { get; }
        void SaveChanges();
    }
}

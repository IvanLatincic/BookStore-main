using BookStore.Data.ViewModels;
using BookStore.Models;

namespace BookStore.Data.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetAllProducts();
        void AddNewProduct(ProductVM productVM, string userId);
        Product GetProductById(int id);
        void UpdateProduct(ProductVM productVM);
        ProductDropDown GetDropDownValues();
    }
}

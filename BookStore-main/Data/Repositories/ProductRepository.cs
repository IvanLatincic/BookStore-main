using BookStore.Data.ViewModels;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public ProductRepository(BookStoreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNewProduct(ProductVM productVM, string userId)
        {
            var newProduct = new Product();
            newProduct.Name = productVM.Name;
            newProduct.Author = productVM.Author;
            newProduct.CategoryId = productVM.CategoryId;
            newProduct.SubCategoryId = productVM.SubCategoryId;
            newProduct.Language = productVM.Language;
            newProduct.State = productVM.State;
            newProduct.Description = productVM.Description;
            newProduct.Price = productVM.Price;
            newProduct.ImageUrl = productVM.ImageUrl;
            newProduct.Stock = true;
            newProduct.UserId = userId;
            //newProduct.ProductPicture = productVM.ProductPicture;

            _dbContext.Products.Add(newProduct);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products.Include(x => x.Category).Include(x => x.SubCategory).Include(x => x.User).ToList();
        }

        public ProductDropDown GetDropDownValues()
        {
            ProductDropDown dropDown = new ProductDropDown
            {
                Categories = _dbContext.Categories.OrderBy(x => x.Name).ToList(),
                SubCategories = _dbContext.SubCategories.OrderBy(x => x.Name).ToList(),
            };

            return dropDown;
        }

        public Product GetProductById(int id)
        {
            return _dbContext.Products.Include(x => x.Category).Include(x => x.SubCategory).Include(x => x.User).FirstOrDefault(x => x.Id == id);
        }

        public void UpdateProduct(ProductVM productVM)
        {
            var existingProduct = _dbContext.Products.Find(productVM.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = productVM.Name;
                existingProduct.Author = productVM.Author;
                existingProduct.CategoryId = productVM.CategoryId;
                existingProduct.SubCategoryId = productVM.SubCategoryId;
                existingProduct.Language = productVM.Language;
                existingProduct.State = productVM.State;
                existingProduct.Description = productVM.Description;
                existingProduct.Price = productVM.Price;
                existingProduct.ImageUrl = productVM.ImageUrl;
                existingProduct.Stock = productVM.Stock;
                //existingProduct.ProductPicture = productVM.ProductPicture;
            }
        }
    }
}

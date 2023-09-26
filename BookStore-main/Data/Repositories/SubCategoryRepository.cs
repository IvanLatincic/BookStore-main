using BookStore.Data.ViewModels;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
    {
        private readonly BookStoreDbContext _dbContext;
        public SubCategoryRepository(BookStoreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNewSubCategory(SubCategoryVM subCategoryVM)
        {
            var newSubCategory = new SubCategory();
            newSubCategory.Name = subCategoryVM.Name;
            newSubCategory.CategoryId = subCategoryVM.CategoryId;

            _dbContext.SubCategories.Add(newSubCategory);
        }

        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            return _dbContext.SubCategories.Include(x => x.Category).ToList();
        }

        public SubCategoryDropDown GetDropDownValues()
        {
            SubCategoryDropDown dropDown = new SubCategoryDropDown
            {
                Categories = _dbContext.Categories.OrderBy(x => x.Name).ToList(),
            };

            return dropDown;
        }

        public SubCategory GetSubCategoryById(int id)
        {
            return _dbContext.SubCategories.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public void UpdateSubCategory(SubCategoryVM subCategoryVM)
        {
            var existingSubCategory = _dbContext.SubCategories.Find(subCategoryVM.Id);
            if (existingSubCategory != null)
            {
                existingSubCategory.Name = subCategoryVM.Name;
                existingSubCategory.CategoryId = subCategoryVM.CategoryId;
                //existingProduct.ProductPicture = productVM.ProductPicture;
            }
        }
    }
}

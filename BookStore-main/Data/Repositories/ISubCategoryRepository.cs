using BookStore.Data.ViewModels;
using BookStore.Models;

namespace BookStore.Data.Repositories
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        IEnumerable<SubCategory> GetAllSubCategories();
        void AddNewSubCategory(SubCategoryVM subCategoryVM);
        SubCategory GetSubCategoryById(int id);
        void UpdateSubCategory(SubCategoryVM subCategoryVM);
        SubCategoryDropDown GetDropDownValues();
    }
}

using BookStore.Models;

namespace BookStore.Data.ViewModels
{
    public class ProductDropDown
    {
        public ProductDropDown()
        {
            Categories = new List<Category>();
            SubCategories = new List<SubCategory>();
        }

        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}

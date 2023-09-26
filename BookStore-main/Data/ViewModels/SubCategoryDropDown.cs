using BookStore.Models;

namespace BookStore.Data.ViewModels
{
    public class SubCategoryDropDown
    {
        public SubCategoryDropDown()
        {
            Categories = new List<Category>();
        }

        public List<Category> Categories { get; set; }
    }
}

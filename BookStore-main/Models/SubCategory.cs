using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Naziv")]
        public string Name { get; set; }
        [Display(Name = "Kategorija")]
        public int CategoryId { get; set; }
        [Display(Name = "Kategorija")]
        public Category Category { get; set; }
    }
}

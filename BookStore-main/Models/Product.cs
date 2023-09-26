using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Naziv Knjige")]
        public string Name { get; set; }
        [Display(Name = "Autor")]
        public string Author { get; set; }
        [Display(Name = "Jezik")]
        public string Language { get; set; }
        [Display(Name = "Stanje")]
        public string State { get; set; }
        [MaxLength(length: 500)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Cijena")]
        public double Price { get; set; }
        [Display(Name = "Slika")]
        public string ImageUrl { get; set; }
        public bool Stock { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [Display(Name = "Kategorija")]
        public Category Category { get; set; }
        public int SubCategoryId { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        [Display(Name = "Podkategorija")]
        public SubCategory SubCategory { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [NotMapped]
        public IFormFile ProductPicture { get; set; }
    }
}

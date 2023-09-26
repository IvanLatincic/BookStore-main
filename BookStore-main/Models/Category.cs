using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv kategorije je obavezan za unos.")]

        [Display(Name = "Naziv kategorije")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Opis kategorije je obavezan za unos.")]
        [Display(Name = "Opis Kategorije")]
        public string Description { get; set; }

        //NavigationalProperty
        public ICollection<Product> Products { get; set; }
    }
}

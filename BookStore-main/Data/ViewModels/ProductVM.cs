using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Data.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv knjige je obavezan za unos.")]
        [Display(Name = "Naziv Knjige")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ime autora je obavezno za unos.")]
        [Display(Name = "Autor")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Jezik knjige je obavezan za unos.")]
        [Display(Name = "Jezik")]
        public string Language { get; set; }
        [Required(ErrorMessage = "Stanje knjige je obavezno za unos.")]
        [Display(Name = "Stanje")]
        public string State { get; set; }
        [Required(ErrorMessage = "Opis knjige je obavezan za unos.")]
        [MaxLength(length: 500)]
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Cijena knjige je obavezna za unos.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Cijena")]
        public double Price { get; set; }
        [Display(Name = "Slika")]
        public string ImageUrl { get; set; }
        [Display(Name = "Stanje")]
        [Required(ErrorMessage = "Unos stanja je obavezan")]
        public bool Stock { get; set; }
        [Display(Name = "Odaberite kategoriju")]
        [Required(ErrorMessage = "Potrebno je odabrati kategoriju")]
        public int CategoryId { get; set; }
        [Display(Name = "Odaberite podkategoriju")]
        [Required(ErrorMessage = "Potrebno je odabrati podkategoriju")]
        public int SubCategoryId { get; set; }

        public string UserId { get; set; }
        [NotMapped]
        public IFormFile ProductPicture { get; set; }
    }
}

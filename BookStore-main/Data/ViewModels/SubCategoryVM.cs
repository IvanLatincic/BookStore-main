using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Data.ViewModels
{
    public class SubCategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv podkategorije je obavezan za unos.")]
        [Display(Name = "Naziv podkategorije")]
        public string Name { get; set; }
        [Display(Name = "Odaberite kategoriju")]
        [Required(ErrorMessage = "Potrebno je odabrati kategoriju")]
        public int CategoryId { get; set; }
    }
}

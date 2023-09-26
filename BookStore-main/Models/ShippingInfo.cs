using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class ShippingInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Display(Name = "Ime")]
        public string Name { get; set; }
        [Display(Name = "Prezime")]
        public string LastName { get; set; }
        [Display(Name = "Adresa")]
        public string Address { get; set; }
        [Display(Name = "Grad/Mjesto")]
        public string City { get; set; }
        [Display(Name = "Poštanski broj")]
        public string ZipCode { get; set; }
        [Display(Name = "Županija")]
        public string County { get; set; }
        [Display(Name = "Broj mobitela")]
        public string Contact { get; set; }
    }
}

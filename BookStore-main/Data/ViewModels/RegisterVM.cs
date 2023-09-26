using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "E-mail adresa")]
        [Required(ErrorMessage = "Email adresa je obavezan za unos")]
        public string EmailAddress { get; set; }
        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezna za unos")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Potvrda lozinke")]
        [Required(ErrorMessage = "Potrebno je potvrditi lozinku")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; }
    }
}

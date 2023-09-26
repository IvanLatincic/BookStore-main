using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail adresa")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}

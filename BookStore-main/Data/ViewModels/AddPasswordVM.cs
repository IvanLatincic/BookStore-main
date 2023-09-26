using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.ViewModels
{
    public class AddPasswordVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; }
    }
}

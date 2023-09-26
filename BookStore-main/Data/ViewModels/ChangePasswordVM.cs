using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.ViewModels
{
    public class ChangePasswordVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        [Display(Name ="Trenutačna lozinka")]
        public string CurrentPassword { get; set; }
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

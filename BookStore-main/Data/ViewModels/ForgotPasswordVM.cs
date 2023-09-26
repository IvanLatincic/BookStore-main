using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email adresa")]
        public string EmailAddress { get; set; }
    }
}

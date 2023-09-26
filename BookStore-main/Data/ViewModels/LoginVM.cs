using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Data.ViewModels
{
    public class LoginVM
    {

        [Display(Name = "E-mail adresa")]
        [Required(ErrorMessage = "Email adresa je obavezna za unos")]
        public string EmailAddress { get; set; }
        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezna za unos")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}

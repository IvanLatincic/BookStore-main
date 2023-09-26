using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Data.ViewModels
{
    public class AddRoleVM
    {
        [Required]
        [Display(Name = "Naziv uloge")]
        public string RoleName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Data.ViewModels
{
    public class EditRoleVM
    {
        public string Id { get; set; }

        [Display(Name = "Uloga")]
        [Required(ErrorMessage = "Naziv uloge je obavezan")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new();
    }
}

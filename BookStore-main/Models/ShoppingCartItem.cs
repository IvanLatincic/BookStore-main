using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        [Display(Name = "Proizvod")]
        public int ProductId { get; set; }
        [Display(Name = "Proizvod")]
        public Product Product { get; set; }
        [Display(Name = "Količina")]
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}

using BookStore.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Utijeku;
        public DateTime OrderPlaced { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public int ShippingInfoId { get; set; }
        public ShippingInfo ShippingInfo { get; set; }

    }
}

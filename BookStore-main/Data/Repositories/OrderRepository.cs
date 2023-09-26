using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly BookStoreDbContext _dbContext;

        public OrderRepository(BookStoreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> GetOrderByUserIdAndUserRole(string userId, string userRole)
        {
            var order = _dbContext.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.User).Include(x => x.ShippingInfo).ToList();

            if (userRole != "Admin")
            {
                order = order.Where(x => x.UserId == userId).ToList();
            }
            return order;
        }

        public void StoreOrder(List<ShoppingCartItem> items, string userId, ShippingInfo shippingInfo)
        {
            var order = new Order()
            {
                UserId = userId,
                OrderPlaced = DateTime.Now,
                OrderItems = new List<OrderItem>(),
                ShippingInfo = new ShippingInfo()
                {
                    Name = shippingInfo.Name,
                    LastName = shippingInfo.LastName,
                    Address= shippingInfo.Address,
                    City = shippingInfo.City,
                    ZipCode = shippingInfo.ZipCode,
                    County = shippingInfo.County,
                    Contact = shippingInfo.Contact
                }
            };


            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    Price = item.Product.Price,
                    ProductId = item.Product.Id
                };
                order.OrderItems.Add(orderItem);
            }
            _dbContext.Orders.Add(order);
        }

        public void CancelOrder(string userId, int orderId)
        {
            var order = _dbContext.Orders.Include(x => x.OrderItems).FirstOrDefault(x => x.Id == orderId);

            if (order != null)
            {
                order.OrderStatus = Enums.OrderStatus.Otkazana;
                _dbContext.Orders.Remove(order);
            }

        }
    }
}

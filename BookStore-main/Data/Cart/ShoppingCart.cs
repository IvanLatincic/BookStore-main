using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Cart
{
    public class ShoppingCart
    {
        private readonly BookStoreDbContext _dbContext;
        public string ShoppingCartId { get; set; }
        public ShoppingCart(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider service)
        {
            var session = service.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            var context = service.GetRequiredService<BookStoreDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public List<ShoppingCartItem> GetCartItems()
        {
            return _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId).Include(x => x.Product).ToList();
        }

        public double GetCartTotal() => _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId).Select(x => x.Product.Price * x.Amount).Sum();
        public int GetCartTotalAmount() => _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId).Select(x => x.Amount).Sum();

        public void AddToCart(Product product)
        {
            var shoppingCartItem = _dbContext.ShoppingCartItems.FirstOrDefault(x => x.Product.Id == product.Id && x.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    ProductId = product.Id,
                    Amount = 1
                };
                _dbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _dbContext.SaveChanges();
        }

        public void RemoveFromCart(Product product)
        {
            var shoppingCartItem = _dbContext.ShoppingCartItems.FirstOrDefault(x => x.Product.Id == product.Id && x.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
                _dbContext.SaveChanges();
            }
        }

        public void ClearCart()
        {
            var items = _dbContext.ShoppingCartItems.Where(x => x.ShoppingCartId == ShoppingCartId).ToList();
            _dbContext.ShoppingCartItems.RemoveRange(items);
            _dbContext.SaveChanges();
        }
    }
}

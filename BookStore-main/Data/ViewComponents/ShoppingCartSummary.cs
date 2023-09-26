using BookStore.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _cart;

        public ShoppingCartSummary(ShoppingCart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            var item = _cart.GetCartTotalAmount();
            ViewBag.Total = _cart.GetCartTotal();
            return View(item);
        }
    }
}

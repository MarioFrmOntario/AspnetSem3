using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aspnet_Projects.Services;
using Aspnet_Project.Models;


namespace Aspnet_Projects.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CartService _cartService;
        private ApplicationDbContext _context;

        public OrdersController(CartService cartService, ApplicationDbContext context, IConfiguration configuration)
        {
            _cartService = cartService;
            _context = context;
        }

        [Authorize()]
        public IActionResult Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = _cartService.GetCart();

            if (cart == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                UserId = userId,
                Total = cart.CartItems.Sum(cartItem => (decimal)(cartItem.Quantity * cartItem.Product.MSRP)),
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductName = cartItem.Product.Name,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.MSRP
                });
            }

            return View("Details", order);
        }

    }
}
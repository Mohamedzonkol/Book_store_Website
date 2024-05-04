using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using Book.Utilites;
using BookWeb.Models;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController(IUnitOfWork unit, ILogger<HomeController> logger) : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Product> productList = unit.Product.GetAll(includeProperty: "Category,ProductImages");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = unit.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shoppingCart.UserId = userId;

            ShoppingCart cartFromDb = unit.ShoppingCart.Get(u => u.UserId == userId &&
                                                                        u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                unit.ShoppingCart.Update(cartFromDb);
                unit.Save();
            }
            else
            {
                //add cart record
                unit.ShoppingCart.Add(shoppingCart);
                unit.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                unit.ShoppingCart.GetAll(u => u.UserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

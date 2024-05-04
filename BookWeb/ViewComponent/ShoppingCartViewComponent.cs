using Book.DataAceess.Repositories.Interfaces;
using Book.Utilites;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BookWeb.ViewComponent
{
    public class ShoppingCartViewComponent(IUnitOfWork unit) : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {

                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        unit.ShoppingCart.GetAll(u => u.UserId == claim.Value).Count());
                }

                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}

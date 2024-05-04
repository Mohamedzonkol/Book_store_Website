using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using Book.Models.ViewModel;
using Book.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController(IUnitOfWork unit, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            RoleVM roleVm = new RoleVM()
            {
                ApplicationUser = unit.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company"),
                RoleList = roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = unit.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            roleVm.ApplicationUser.Role = userManager.GetRolesAsync(unit.ApplicationUser.Get(u => u.Id == userId))
                .GetAwaiter().GetResult().FirstOrDefault()!;
            return View(roleVm);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleVM roleVm)
        {
            string oldRole = userManager.GetRolesAsync(unit.ApplicationUser.Get(u => u.Id == roleVm.ApplicationUser.Id))
                .GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = unit.ApplicationUser.Get(u => u.Id == roleVm.ApplicationUser.Id);


            if (!(roleVm.ApplicationUser.Role == oldRole))
            {
                //a role was updated
                if (roleVm.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleVm.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                unit.ApplicationUser.Update(applicationUser);
                unit.Save();

                userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                userManager.AddToRoleAsync(applicationUser, roleVm.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            else
            {
                if (oldRole == SD.Role_Company && applicationUser.CompanyId != roleVm.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = roleVm.ApplicationUser.CompanyId;
                    unit.ApplicationUser.Update(applicationUser);
                    unit.Save();
                }
            }

            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = unit.ApplicationUser.GetAll(includeProperty: "Company").ToList();
            foreach (var user in objUserList)
            {
                user.Role = userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()!;
                user.Company ??= new Company()
                {
                    Name = ""
                };
            }
            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = unit.ApplicationUser.Get(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            unit.ApplicationUser.Update(objFromDb);
            unit.Save();
            unit.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }

        #endregion
    }
}

using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using Book.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController(IUnitOfWork unit) : Controller
    {
        public IActionResult Index()
        {
            List<Company> companies = unit.Company.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {

            if (id is null or 0)
            {
                return View(new Company()); //Create
            }
            else
            {
                //update
                Company company = unit.Company.Get(x => x.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    unit.Company.Add(company);
                }
                else
                {
                    unit.Company.Update(company);
                }
                unit.Save();
                TempData["success"] = "Company created/updated successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }
        #region Api Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = unit.Company.GetAll().ToList();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company company = unit.Company.Get(x => x.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unit.Company.Remove(company);
            unit.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}

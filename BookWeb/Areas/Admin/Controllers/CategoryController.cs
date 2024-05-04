using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using Book.Utilites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController(IUnitOfWork unit) : Controller
    {
        public IActionResult Index()
        {
            List<Category> categories = unit.Category.GetAll().ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                unit.Category.Add(category);
                unit.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound("This Category Is Not Found");
            }
            Category category = unit.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound("This Category Is Not Found");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category oldCategory)
        {
            if (ModelState.IsValid)
            {
                unit.Category.Update(oldCategory);
                unit.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
            {
                return NotFound("This Category Is Not Found");
            }
            Category? category = unit.Category.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound("This Category Is Not Found");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Category? obj = unit.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            unit.Category.Remove(obj);
            unit.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

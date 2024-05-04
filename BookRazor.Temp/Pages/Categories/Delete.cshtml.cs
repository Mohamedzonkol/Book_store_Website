using BookRazor.Temp.Data;
using BookRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookRazor.Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel(AppDbContext context) : PageModel
    {
        public Category Category { get; set; }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = context.Categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            Category? obj = context.Categories.Find(Category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            context.Categories.Remove(obj);
            context.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }
    }
}

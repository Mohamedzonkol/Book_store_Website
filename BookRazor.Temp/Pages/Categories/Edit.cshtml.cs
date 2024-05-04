using BookRazor.Temp.Data;
using BookRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookRazor.Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel(AppDbContext context) : PageModel
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
            if (ModelState.IsValid)
            {
                context.Categories.Update(Category);
                context.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}

using BookRazor.Temp.Data;
using BookRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookRazor.Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel(AppDbContext context) : PageModel
    {
        public Category Category { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            context.Categories.Add(Category);
            context.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToPage("Index");
        }
    }
}

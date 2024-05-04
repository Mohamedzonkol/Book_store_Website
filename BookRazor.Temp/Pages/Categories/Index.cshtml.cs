using BookRazor.Temp.Data;
using BookRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookRazor.Temp.Pages.Categories
{
    public class Index1Model(AppDbContext context) : PageModel
    {
        public List<Category> CategoryList { get; set; }
        public void OnGet()
        {
            CategoryList = context.Categories.ToList();
        }
    }
}

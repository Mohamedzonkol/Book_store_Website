using Microsoft.AspNetCore.Mvc.Rendering;

namespace Book.Models.ViewModel
{
    public class RoleVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}

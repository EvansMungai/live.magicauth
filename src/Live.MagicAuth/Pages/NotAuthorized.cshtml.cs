using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Live.MagicAuth.Pages
{
    [Authorize]
    public class NotAuthorizedModel : PageModel
    {
        public string Role { get; set; }
        public IActionResult OnGet()
        {
            Role = User.FindFirstValue(ClaimTypes.Role) ?? "user";

            return Page();
        }
    }
}

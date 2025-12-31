using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Live.MagicAuth.Pages
{
    [Authorize(Roles = "admin")]
    public class AdminDashboardModel : PageModel
    {
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public IActionResult OnGet()
        {
            UserEmail = User.FindFirstValue(ClaimTypes.Email) ?? "Not Available";
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Not Available";
            DisplayName = User.FindFirstValue(ClaimTypes.Name) ?? "Not available";
            Role = User.FindFirstValue(ClaimTypes.Role) ?? "Not available";

            return Page();
        }
    }
}

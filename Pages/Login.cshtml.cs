using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SMS.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // simple static authentication for demo/testing as requested
            if (string.Equals(Email, "admin@mail.com", System.StringComparison.OrdinalIgnoreCase)
                && Password == "123")
            {
                // redirect to the static copy of the admin dashboard
                return Redirect("/admin/dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return Page();
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace SMS.Pages
{
    public class VerifyCodeModel : PageModel
    {
        private readonly IMemoryCache _cache;

        public VerifyCodeModel(IMemoryCache cache)
        {
            _cache = cache;
        }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Code { get; set; }

        public void OnGet(string? email)
        {
            Email = email;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Code))
            {
                ModelState.AddModelError(string.Empty, "Email and code are required.");
                return Page();
            }

            var cacheKey = $"ForgotCode:{Email}";
            if (!_cache.TryGetValue<string>(cacheKey, out var storedCode))
            {
                ModelState.AddModelError(string.Empty, "No verification code found or it has expired.");
                return Page();
            }

            if (!string.Equals(storedCode, Code, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "The code is invalid. Please check and try again.");
                return Page();
            }

            // valid - remove the cached code
            _cache.Remove(cacheKey);

            // For now, redirect to login where user can reset password (or implement reset flow next)
            return RedirectToPage("/Login", new { verified = true, email = Email });
        }
    }
}

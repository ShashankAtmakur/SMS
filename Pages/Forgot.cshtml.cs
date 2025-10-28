using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using SMS.Services;

namespace SMS.Pages
{
    public class ForgotModel : PageModel
    {
        private readonly IMemoryCache _cache;
        private readonly EmailService _emailService;

        public ForgotModel(IMemoryCache cache, EmailService emailService)
        {
            _cache = cache;
            _emailService = emailService;
        }

    [BindProperty]
    public string? Email { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ModelState.AddModelError(string.Empty, "Please provide a valid email address.");
                return Page();
            }

            // generate 6-digit code
            var rng = new Random();
            var code = rng.Next(100000, 999999).ToString();

            // store in memory cache for 10 minutes
            var cacheKey = $"ForgotCode:{Email}";
            _cache.Set(cacheKey, code, TimeSpan.FromMinutes(10));

            try
            {
                await _emailService.SendVerificationCodeAsync(Email, code);
            }
            catch (Exception ex)
            {
                // log or surface error
                ModelState.AddModelError(string.Empty, "Failed to send email. Check SMTP configuration.");
                Console.Error.WriteLine(ex);
                return Page();
            }

            // Redirect to verify page
            return RedirectToPage("/VerifyCode", new { email = Email });
        }
    }
}

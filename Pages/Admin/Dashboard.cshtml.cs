using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SMS.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        public string RawHtml { get; set; } = string.Empty;

        public void OnGet()
        {
            // Read the dashboard HTML from the wwwroot admin folder and serve it unchanged except for asset path rewrites and icon CDN fallback.
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", "dashboard.html");
            if (System.IO.File.Exists(filePath))
            {
                var html = System.IO.File.ReadAllText(filePath);
                // ensure asset paths point to the admin static folder
                html = html.Replace("./School Management_files/", "/admin/School Management_files/");

                // Add CDN fallbacks for icon fonts to ensure icons render even if local font files are missing.
                var headClose = "</head>";
                var cdnLinks = "<link href=\"https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css\" rel=\"stylesheet\">\n<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css\">\n";
                if (html.Contains(headClose))
                {
                    html = html.Replace(headClose, cdnLinks + headClose);
                }

                RawHtml = html;
            }
            else
            {
                RawHtml = "<h2>Dashboard file not found on server.</h2>";
            }
        }
    }
}

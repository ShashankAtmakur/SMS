using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SMS.Services
{
    /// <summary>
    /// Simple email sender service that uses SMTP settings from configuration.
    /// Configure in appsettings.json or environment variables under the "Smtp" section:
    /// {
    ///   "Smtp": {
    ///     "Host": "smtp.gmail.com",
    ///     "Port": 587,
    ///     "User": "your@email",
    ///     "Pass": "app-password",
    ///     "From": "your@email"
    ///   }
    /// }
    /// For Gmail use an app-specific password and enable SMTP access.
    /// </summary>
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            var smtpSection = _config.GetSection("Smtp");
            var host = smtpSection.GetValue<string>("Host") ?? "smtp.gmail.com";
            var port = smtpSection.GetValue<int?>("Port") ?? 587;
            var user = smtpSection.GetValue<string>("User");
            var pass = smtpSection.GetValue<string>("Pass");
            var from = smtpSection.GetValue<string>("From") ?? user ?? "no-reply@example.com";

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass)
            };

            var subject = "Your verification code";
            var body = $@"<p>Your verification code is <strong>{code}</strong>. It will expire in 10 minutes.</p>";

            using var msg = new MailMessage(from, toEmail, subject, body) { IsBodyHtml = true };

            // Note: SmtpClient.SendMailAsync can throw if credentials are missing or incorrect.
            await client.SendMailAsync(msg);
        }
    }
}

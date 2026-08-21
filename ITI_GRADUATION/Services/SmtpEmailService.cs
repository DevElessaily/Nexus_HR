using System.Net;
using System.Net.Mail;
using ITI_GRADUATION.Models;
using Microsoft.Extensions.Options;

namespace ITI_GRADUATION.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IOptions<EmailSettings> settings, ILogger<SmtpEmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendNewEmployeeNotificationAsync(string employeeFullName, string employeeEmail, string? departmentName, string? jobTitleName)
        {
            // If email isn't configured yet, skip quietly instead of crashing the Create action.
            if (string.IsNullOrWhiteSpace(_settings.SenderEmail) || string.IsNullOrWhiteSpace(_settings.SenderPassword) || string.IsNullOrWhiteSpace(_settings.NotificationRecipient))
            {
                _logger.LogWarning("Email notification skipped: EmailSettings are not configured in appsettings.json.");
                return;
            }

            try
            {
                using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_settings.SenderEmail, _settings.SenderPassword)
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                    Subject = "New Employee Added — Nexus HR",
                    IsBodyHtml = true,
                    Body = $@"
                        <div style='font-family:Arial,sans-serif;font-size:15px;color:#1E293B;'>
                            <h2 style='color:#059669;'>A new employee has been added</h2>
                            <p><strong>Name:</strong> {WebUtility.HtmlEncode(employeeFullName)}</p>
                            <p><strong>Email:</strong> {WebUtility.HtmlEncode(employeeEmail)}</p>
                            <p><strong>Department:</strong> {WebUtility.HtmlEncode(departmentName ?? "—")}</p>
                            <p><strong>Job Title:</strong> {WebUtility.HtmlEncode(jobTitleName ?? "—")}</p>
                            <hr style='border:none;border-top:1px solid #E2E8F0;margin:16px 0;' />
                            <p style='color:#64748B;font-size:13px;'>Sent automatically by Nexus HR.</p>
                        </div>"
                };
                message.To.Add(_settings.NotificationRecipient);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Never let a mail failure break the employee creation flow.
                _logger.LogError(ex, "Failed to send new employee notification email.");
            }
        }
    }
}

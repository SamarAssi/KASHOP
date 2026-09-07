using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace KASHOP.BLL;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Task SendEmailAsync(
        string email,
        string subject,
        string message
    )
    {
        var client = new SmtpClient(
            _configuration["EmailSettings:Host"],
            int.Parse(_configuration["EmailSettings:Port"])
        )
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:Email"],
                _configuration["EmailSettings:Password"]
            )
        };

        return client.SendMailAsync(
            new MailMessage(
                from: _configuration["EmailSettings:Email"],
                to: email,
                subject: subject,
                body: message
            )
            {
                IsBodyHtml = true
            }
        );
    }
}

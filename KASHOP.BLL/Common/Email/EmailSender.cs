using System.Net;
using System.Net.Mail;

namespace KASHOP.BLL;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(
        string email,
        string subject,
        string message
    )
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                "samar.assi2001@gmail.com",
                "zirn cxbm pdhr jvis"
            )
        };

        return client.SendMailAsync(
            new MailMessage(
                from: "samar.assi2001@gmail.com",
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

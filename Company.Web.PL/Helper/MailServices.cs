using Company.Web.PL.Helpers;
using Company.Web.PL.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Company.Web.PL.Helper
{
    public class MailServices(IOptions<MailSettings> options) : IMailServices
    {
        public bool SendEmail(Email email)
        {
            try
            {
                // Create message
                var mail = new MimeMessage();
                mail.Subject = email.Subject;
                mail.From.Add(new MailboxAddress(options.Value.DisplayName, options.Value.Email));
                mail.To.Add(MailboxAddress.Parse(email.To));

                var bodybuilder = new BodyBuilder();
                bodybuilder.TextBody = email.Body;
                mail.Body = bodybuilder.ToMessageBody();

                // Establish connection
                using var smtp = new SmtpClient();
                smtp.Connect(options.Value.Host, options.Value.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(options.Value.Email, options.Value.Password);
                // Send message
                smtp.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

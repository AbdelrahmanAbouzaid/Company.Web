using System.Net;
using System.Net.Mail;

namespace Company.Web.PL.Helpers
{
    public static class EmailSetting
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("a.abouzaid404@gmail.com", "rdnpfetwiozufjim");
                client.Send("a.abouzaid404@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

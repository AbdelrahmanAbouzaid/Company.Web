using Company.Web.PL.Helpers;

namespace Company.Web.PL
{
    public interface IMailServices
    {
        public bool SendEmail(Email email);
    }
}

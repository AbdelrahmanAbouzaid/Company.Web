using Company.Web.PL.Helpers;

namespace Company.Web.PL.Helper
{
    public interface IMailServices
    {
        public bool SendEmail(Email email);
    }
}

using System.ComponentModel.DataAnnotations;

namespace Company.Web.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email Is Required!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}

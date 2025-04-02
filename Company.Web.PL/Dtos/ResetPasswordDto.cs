using System.ComponentModel.DataAnnotations;

namespace Company.Web.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password Is Required!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match Password")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required (ErrorMessage =" New Password is Required")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("NewPassword" , ErrorMessage ="Password Does't Match")]
        public string ConfirmNewPassword { get; set; }
    }
}

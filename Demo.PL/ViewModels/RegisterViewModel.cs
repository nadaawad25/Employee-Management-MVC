using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First Name is Required ")]
        public String FName { get; set; }
        [Required(ErrorMessage = "Last Name is Required ")]
        public String LName { get; set; }

        [Required(ErrorMessage = "Email is Required ")]
        [EmailAddress(ErrorMessage ="Invalid Email")]

        public String Email { get; set; }
        [Required(ErrorMessage = " Password Is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
       

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password" ,ErrorMessage ="Password Does't Match ")]
        public string PasswordConfirm { get; set; }

        public bool IsAgree { get; set; }


    }
}

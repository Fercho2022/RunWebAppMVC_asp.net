using System.ComponentModel.DataAnnotations;

namespace RunWebAppGroup.ViewModels
{
    public class RegisterViewModel
    {

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter and one special character (@, $, !, %, *, ?, &).")]
        public string Password { get; set; }


        [Display(Name= "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

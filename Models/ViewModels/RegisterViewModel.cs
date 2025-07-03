using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    /*
    This class is used to store user input for registration, either by user or admin.
    Class 'User' is not used to store user input because it inherits the IdentityUser, which 
    does not contain validation attributes. 
    RegisterViewModel therefore take the responsibilities of validating the user inputs.
    Once the dotnet validate user inputs, the username/psw value are transferred to the User object.
    */
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
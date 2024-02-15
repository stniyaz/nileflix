using System.ComponentModel.DataAnnotations;

namespace Movie.Business.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "*Please don't leave the first name line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the last name line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the last name line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [MinLength(2, ErrorMessage = "Please enter minimum 2 characthers.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the email line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Please don't leave the password line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "*Please don't leave the confirm password line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [Compare("Password", ErrorMessage = "*Please enter the confirmation password correctly, the passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public bool Privacy { get; set; }
        public bool RememberMe { get; set; }
    }
}

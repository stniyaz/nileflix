using System.ComponentModel.DataAnnotations;

namespace Movie.Business.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "*Please don't leave the email(username) line blank.")]
        [StringLength(100, ErrorMessage = "*Please enter maxiumum 100 characters.")]
        public string UsernameOrMail { get; set; }
        [Required(ErrorMessage = "*Please don't leave the password line blank.")]
        [StringLength(100, ErrorMessage = "*Please enter maxiumum 100 characters.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

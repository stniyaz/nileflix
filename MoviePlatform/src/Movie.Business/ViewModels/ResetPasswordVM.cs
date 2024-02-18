using System.ComponentModel.DataAnnotations;

namespace Movie.Business.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [MinLength(8, ErrorMessage = "*Please enter minimum 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [MinLength(8, ErrorMessage = "*Please enter minimum 8 characters.")]
        [Compare("Password", ErrorMessage = "*Please enter the confirmation password correctly, the passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

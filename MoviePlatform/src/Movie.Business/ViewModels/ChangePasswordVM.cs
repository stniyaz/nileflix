using System.ComponentModel.DataAnnotations;

namespace Movie.Business.ViewModels
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "*Please don't leave the password line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "*Please don't leave the password line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "*Please don't leave the confirm password line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [Compare("NewPassword", ErrorMessage = "*Please enter the confirmation password correctly, the passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}

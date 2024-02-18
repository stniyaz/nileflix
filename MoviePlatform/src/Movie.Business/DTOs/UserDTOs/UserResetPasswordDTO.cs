using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.UserDTOs
{
    public class UserResetPasswordDTO
    {
        [Required(ErrorMessage = "*Please don't leave the email line blank.")]
        [StringLength(100, ErrorMessage = "*Please enter maxiumum 100 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsSent { get; set; } = false;
    }
}

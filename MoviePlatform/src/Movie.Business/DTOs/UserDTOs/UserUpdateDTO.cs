using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "*Please don't leave the first name line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the last name line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the username line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [MinLength(2, ErrorMessage = "Please enter minimum 2 characthers.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*Please don't leave the email line blank.")]
        [StringLength(50, ErrorMessage = "*Please enter maxiumum 50 characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string RoleId { get; set; }
        public bool IsBanned { get; set; }
        public bool IsPremium { get; set; }
    }
}

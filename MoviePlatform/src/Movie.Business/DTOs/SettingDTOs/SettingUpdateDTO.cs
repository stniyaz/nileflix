using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.SettingDTOs
{
    public class SettingUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Please do not leave the value line blank.")]
        [StringLength(maximumLength: 100, ErrorMessage = "Please enter a minimum of 1 and a maximum of 100.", MinimumLength = 1)]
        public string Value { get; set; }
    }
}

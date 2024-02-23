using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.LiveDTOs
{
    public class LiveUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Please do not leave the name line blank.")]
        [StringLength(maximumLength: 50, ErrorMessage = "Please enter a minimum of 2 and a maximum of 50.", MinimumLength = 2)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*Please do not leave the title line blank.")]
        [StringLength(maximumLength: 100, ErrorMessage = "Please enter a minimum of 2 and a maximum of 50.", MinimumLength = 2)]
        public string Url { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}

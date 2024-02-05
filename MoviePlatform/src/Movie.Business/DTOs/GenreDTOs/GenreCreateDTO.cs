using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.GenreDTOs
{
    public class GenreCreateDTO
    {
        [Required(ErrorMessage = "Please do not leave the name line blank.")]
        [StringLength(maximumLength: 50,ErrorMessage = "Please enter a minimum of 2 and a maximum of 10.", MinimumLength =2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please upload a photo.")]
        public IFormFile ImageFile { get; set; }
    }
}

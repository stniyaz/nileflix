using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.MovieDTOs
{
    public class MovieUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Please do not leave the title line blank.")]
        [StringLength(maximumLength: 255, ErrorMessage = "*Please enter a minimum of 1 and a maximum of 255.", MinimumLength = 1)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*Please do not leave the description line blank.")]
        [StringLength(maximumLength: 1024, ErrorMessage = "*Please enter a minimum of 10 and a maximum of 1024.", MinimumLength = 10)]
        public string Description { get; set; }
        [Required(ErrorMessage = "*Please do not leave the movielong line blank.")]
        [StringLength(maximumLength: 20, ErrorMessage = "*Please enter a minimum of 10 and a maximum of 1024.", MinimumLength = 2)]
        public string MovieLong { get; set; }
        [Required(ErrorMessage = "*Please do not leave the ReleaseYear line blank.")]
        public int ReleaseYear { get; set; }
        [Required(ErrorMessage = "*Please do not leave the AgeLimit line blank.")]
        [Range(0, 18, ErrorMessage = "Age must be between 0 and 18")]
        public int AgeLimit { get; set; }
        public bool IsPopular { get; set; }
        public bool IsNewst { get; set; }


        public IFormFile? CoverImage { get; set; }
        public List<IFormFile>? OtherImages { get; set; }
        public IFormFile? TrailerVideo { get; set; }
        public IFormFile? FullVideo { get; set; }


        public int CountryId { get; set; }
        [Required(ErrorMessage = "*Please do not leave the genre line blank.")]
        public List<int> GenreIds { get; set; }
        [Required(ErrorMessage = "*Please do not leave the photos line blank.")]
        public List<int> ImageIds { get; set; }
    }
}

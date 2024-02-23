using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.ComentDTOs
{

    public class CommentCreateDTO
    {
        public int MovieId { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "*Please enter maxiumum 1000 characters.")]
        [MinLength(10, ErrorMessage = "*Please enter minimum 10 characters.")]
        public string Text { get; set; }
    }
}

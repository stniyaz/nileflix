using System.ComponentModel.DataAnnotations;

namespace Movie.Business.DTOs.ComentDTOs
{
    public class CommentUpdateDTO
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public bool IsSpoiler { get; set; }
    }
}

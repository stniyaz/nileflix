namespace Movie.Core.Models
{
    public class CommentReaction : BaseEntity
    {
        public string AppUserId { get; set; }
        public int CommentId { get; set; }
        public bool IsLike { get; set; }

        public AppUser AppUser { get; set; }
        public Comment Comment { get; set; }

    }
}

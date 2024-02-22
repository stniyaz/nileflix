namespace Movie.Core.Models
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public List<Comment> Replies { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public List<CommentReaction> CommentReactions { get; set; }
    }
}

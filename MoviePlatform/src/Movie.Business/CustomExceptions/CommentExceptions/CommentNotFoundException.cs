namespace Movie.Business.CustomExceptions.CommentExceptions
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException()
        {
        }

        public CommentNotFoundException(string? message) : base(message)
        {
        }
    }
}

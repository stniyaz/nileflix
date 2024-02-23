namespace Movie.Business.CustomExceptions.CommentExceptions
{
    public class CommentContainArgoException : Exception
    {
        public string PropertyName { get; set; }
        public CommentContainArgoException()
        {
        }

        public CommentContainArgoException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

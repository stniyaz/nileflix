namespace Movie.Business.CustomExceptions.MoiveExceptions
{
    public class InvalidSubtitileContentTypeException : Exception
    {
        public string PropertyName { get; set; }
        public InvalidSubtitileContentTypeException()
        {
        }

        public InvalidSubtitileContentTypeException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

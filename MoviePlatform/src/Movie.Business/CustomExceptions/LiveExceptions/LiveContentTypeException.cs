namespace Movie.Business.CustomExceptions.LiveExceptions
{
    public class LiveContentTypeException : Exception
    {
        public string PropertyName { get; set; }
        public LiveContentTypeException()
        {
        }

        public LiveContentTypeException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

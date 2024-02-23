namespace Movie.Business.CustomExceptions.LiveExceptions
{
    public class LiveImageLengthException : Exception
    {
        public string PropertyName { get; set; }
        public LiveImageLengthException()
        {
        }

        public LiveImageLengthException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

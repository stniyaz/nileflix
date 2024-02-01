namespace Movie.Business.CustomExceptions.GenreExceptions
{
    public class GenreImageContentTypeException : Exception
    {
        public string PropertyName { get; set; }
        public GenreImageContentTypeException()
        {
        }

        public GenreImageContentTypeException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

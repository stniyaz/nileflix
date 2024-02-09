namespace Movie.Business.CustomExceptions.CommonExceptions
{
    public class InvalidPageException : Exception
    {
        public InvalidPageException()
        {
        }

        public InvalidPageException(string? message) : base(message)
        {
        }
    }
}

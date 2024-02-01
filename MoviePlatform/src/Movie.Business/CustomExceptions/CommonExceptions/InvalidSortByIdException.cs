namespace Movie.Business.CustomExceptions.CommonExceptions
{
    public class InvalidSortByIdException : Exception
    {
        public InvalidSortByIdException()
        {
        }

        public InvalidSortByIdException(string? message) : base(message)
        {
        }
    }
}

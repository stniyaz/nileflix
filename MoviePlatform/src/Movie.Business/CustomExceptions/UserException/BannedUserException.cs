namespace Movie.Business.CustomExceptions.UserException
{
    public class BannedUserException : Exception
    {
        public BannedUserException()
        {
        }

        public BannedUserException(string? message) : base(message)
        {
        }
    }
}

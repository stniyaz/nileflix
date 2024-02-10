namespace Movie.Business.CustomExceptions.UserException
{
    public class LockedUserException : Exception
    {
        public LockedUserException()
        {
        }

        public LockedUserException(string? message) : base(message)
        {
        }
    }
}

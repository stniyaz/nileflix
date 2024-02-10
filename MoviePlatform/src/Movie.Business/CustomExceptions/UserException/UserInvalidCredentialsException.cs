namespace Movie.Business.CustomExceptions.UserException
{
    public class UserInvalidCredentialsException : Exception
    {
        public UserInvalidCredentialsException()
        {
        }

        public UserInvalidCredentialsException(string? message) : base(message)
        {
        }
    }
}

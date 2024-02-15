namespace Movie.Business.CustomExceptions.UserException
{
    public class UserInvalidCredentialsException : Exception
    {
        public string PropertyName { get; set; }
        public UserInvalidCredentialsException()
        {
        }

        public UserInvalidCredentialsException(string? message) : base(message)
        {
        }
        public UserInvalidCredentialsException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }

    }
}

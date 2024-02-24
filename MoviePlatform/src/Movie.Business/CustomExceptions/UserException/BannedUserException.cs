namespace Movie.Business.CustomExceptions.UserException
{
    public class BannedUserException : Exception
    {
        public string PropertyName { get; set; }
        public BannedUserException()
        {
        }

        public BannedUserException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }

        public BannedUserException(string? message) : base(message)
        {
        }
    }
}

namespace Movie.Business.CustomExceptions.UserException
{
    public class UserNotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string? message) : base(message)
        {
        }
        public UserNotFoundException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

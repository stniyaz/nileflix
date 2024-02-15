namespace Movie.Business.CustomExceptions.UserException
{
    public class InvalidRoleIdException : Exception
    {
        public string PropertyName { get; set; }
        public InvalidRoleIdException()
        {
        }

        public InvalidRoleIdException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

namespace Movie.Business.CustomExceptions.UserException
{
    public class UnexceptedException : Exception
    {
        public string PropertyName { get; set; }
        public UnexceptedException()
        {
        }

        public UnexceptedException(string? message) : base(message)
        {
        }
        public UnexceptedException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

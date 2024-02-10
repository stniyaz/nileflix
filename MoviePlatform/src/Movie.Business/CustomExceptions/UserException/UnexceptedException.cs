namespace Movie.Business.CustomExceptions.UserException
{
    public class UnexceptedException : Exception
    {
        public UnexceptedException()
        {
        }

        public UnexceptedException(string? message) : base(message)
        {
        }
    }
}

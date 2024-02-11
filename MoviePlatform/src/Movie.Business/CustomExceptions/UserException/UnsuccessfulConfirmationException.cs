namespace Movie.Business.CustomExceptions.UserException
{
    public class UnsuccessfulConfirmationException : Exception
    {
        public UnsuccessfulConfirmationException()
        {
        }

        public UnsuccessfulConfirmationException(string? message) : base(message)
        {
        }
    }
}

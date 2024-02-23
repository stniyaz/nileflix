namespace Movie.Business.CustomExceptions.LiveExceptions
{
    public class LiveNotFoundException : Exception
    {
        public LiveNotFoundException()
        {
        }

        public LiveNotFoundException(string? message) : base(message)
        {
        }
    }
}

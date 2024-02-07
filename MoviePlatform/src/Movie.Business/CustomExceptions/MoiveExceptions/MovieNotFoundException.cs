namespace Movie.Business.CustomExceptions.MoiveExceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException()
        {
        }

        public MovieNotFoundException(string? message) : base(message)
        {
        }
    }
}

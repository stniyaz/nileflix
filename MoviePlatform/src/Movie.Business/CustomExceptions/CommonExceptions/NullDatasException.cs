namespace Movie.Business.CustomExceptions.CommonExceptions
{
    public class NullDatasException : Exception
    {
        public NullDatasException()
        {
        }

        public NullDatasException(string? message) : base(message)
        {
        }
    }
}

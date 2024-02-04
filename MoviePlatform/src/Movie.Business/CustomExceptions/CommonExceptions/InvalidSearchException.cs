namespace Movie.Business.CustomExceptions.CommonExceptions
{
	public class InvalidSearchException : Exception
	{
		public InvalidSearchException()
		{
		}

		public InvalidSearchException(string? message) : base(message)
		{
		}
	}
}

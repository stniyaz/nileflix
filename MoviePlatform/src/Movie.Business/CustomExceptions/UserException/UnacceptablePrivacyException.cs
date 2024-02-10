namespace Movie.Business.CustomExceptions.UserException
{
	public class UnacceptablePrivacyException : Exception
	{
		public UnacceptablePrivacyException()
		{
		}

		public UnacceptablePrivacyException(string? message) : base(message)
		{
		}
	}
}

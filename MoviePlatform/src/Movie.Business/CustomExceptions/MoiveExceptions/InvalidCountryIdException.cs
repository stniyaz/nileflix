namespace Movie.Business.CustomExceptions.MoiveExceptions
{
	public class InvalidCountryIdException : Exception
	{
		public string PropertyName { get; set; }
		public InvalidCountryIdException()
		{
		}

		public InvalidCountryIdException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

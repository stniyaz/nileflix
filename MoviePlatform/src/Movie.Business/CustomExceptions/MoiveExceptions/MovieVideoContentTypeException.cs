namespace Movie.Business.CustomExceptions.MoiveExceptions
{
	public class MovieVideoContentTypeException : Exception
	{
		public string PropertyName { get; set; }
		public MovieVideoContentTypeException()
		{
		}

		public MovieVideoContentTypeException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

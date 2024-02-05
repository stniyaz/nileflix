namespace Movie.Business.CustomExceptions.MoiveExceptions
{
	public class MovieImageLengthException : Exception
	{
		public string PropertyName { get; set; }
		public MovieImageLengthException()
		{
		}

		public MovieImageLengthException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

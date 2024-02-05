namespace Movie.Business.CustomExceptions.MoiveExceptions
{
	public class MovieImageContentTypeException : Exception
	{
		public string PropertyName { get; set; }
		public MovieImageContentTypeException()
		{
		}

		public MovieImageContentTypeException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

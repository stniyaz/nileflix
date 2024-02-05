namespace Movie.Business.CustomExceptions.MoiveExceptions
{
	public class InvalidGenreIdException : Exception
	{
		public string PropertyName { get; set; }
		public InvalidGenreIdException()
		{
		}

		public InvalidGenreIdException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

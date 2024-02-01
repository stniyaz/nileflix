namespace Movie.Business.CustomExceptions.GenreExceptions
{
	public class ExistGenreException : Exception
	{
		public string PropertyName { get; set; }
		public ExistGenreException()
		{
		}

		public ExistGenreException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

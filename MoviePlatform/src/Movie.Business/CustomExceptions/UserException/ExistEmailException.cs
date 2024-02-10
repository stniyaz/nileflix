namespace Movie.Business.CustomExceptions.UserException
{
	public class ExistEmailException : Exception
	{
		public string PropertyName { get; set; }
		public ExistEmailException()
		{
		}

		public ExistEmailException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

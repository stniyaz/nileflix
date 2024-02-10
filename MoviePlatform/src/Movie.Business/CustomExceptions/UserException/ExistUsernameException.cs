namespace Movie.Business.CustomExceptions.UserException
{
	public class ExistUsernameException : Exception
	{
		public string PropertyName { get; set; }
		public ExistUsernameException()
		{
		}

		public ExistUsernameException(string propertyName, string? message) : base(message)
		{
			PropertyName = propertyName;
		}
	}
}

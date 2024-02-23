namespace Movie.Business.CustomExceptions.SettingExceptions
{
    public class SettingNotFoundException : Exception
    {
        public SettingNotFoundException()
        {
        }

        public SettingNotFoundException(string? message) : base(message)
        {
        }
    }
}

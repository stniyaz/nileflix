using System.Reflection.Metadata.Ecma335;

namespace Movie.Business.ViewModels
{
    public class ChangeMailVM
    {
        public string UserId { get; set; }
        public string NewMail { get; set; }
        public string Token { get; set; }
        public bool IsChanged { get; set; }
    }
}

using System.Reflection.Metadata.Ecma335;

namespace Movie.Core.Models
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

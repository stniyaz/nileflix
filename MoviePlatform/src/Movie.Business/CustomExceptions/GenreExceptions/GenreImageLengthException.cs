using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Business.CustomExceptions.GenreExceptions
{
    public class GenreImageLengthException : Exception
    {
        public string PropertyName { get; set; }
        public GenreImageLengthException()
        {
        }

        public GenreImageLengthException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}

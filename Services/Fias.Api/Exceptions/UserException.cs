using System.Net;

namespace Fias.Api.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }

        public UserException(string message, HttpStatusCode httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; }
    }
}

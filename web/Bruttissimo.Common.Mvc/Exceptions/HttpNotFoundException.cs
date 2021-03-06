using System;
using System.Net;
using System.Web;

namespace Bruttissimo.Common.Mvc.Exceptions
{
    public class HttpNotFoundException : HttpException
    {
        public HttpNotFoundException()
            : this(null, null)
        {
        }

        public HttpNotFoundException(string message)
            : this(message, null)
        {
        }

        public HttpNotFoundException(string message, Exception innerException)
            : base((int)HttpStatusCode.NotFound, message, innerException)
        {
        }
    }
}

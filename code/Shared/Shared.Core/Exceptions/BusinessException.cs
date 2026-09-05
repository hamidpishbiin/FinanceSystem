using System.Net;

namespace Shared.Core.Exceptions
{
    public class BusinessException : Exception
    {
        private readonly int? _explicitCode;

        public int Code => _explicitCode ?? DefaultCode;

        protected virtual int DefaultCode => 0;

        public HttpStatusCode StatusCode { get; }
        public string ExceptionMessage { get; }

        public BusinessException(
            int code,
            string message = "",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(message, statusCode)
        {
            _explicitCode = code;
        }

        public BusinessException(
            string message = "",
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            ExceptionMessage = message;
            StatusCode = statusCode;
        }
    }
}

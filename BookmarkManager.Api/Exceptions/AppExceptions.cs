
namespace BookmarkManager.Api.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; set; }
        protected  AppException() { }
        protected AppException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

    }

    public class NotFoundException : AppException
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with id '{key}' was not found", 404) { }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(message, 400) { }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, 409) { }
    }
}

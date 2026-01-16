namespace ExpenseTracker.Api.Exceptions
{
    public sealed class UnauthorizedException(string message) : AppException(message)
    {
        public override int StatusCode => StatusCodes.Status401Unauthorized;
    }
}

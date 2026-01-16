namespace ExpenseTracker.Api.Exceptions
{
    public sealed class ValidationException(string message) : AppException(message)
    {
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}

namespace MicroInventory.Shared.Common.Response
{
    public class Result : IResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? StatusCodes { get; set; }

        public Result(bool success)
        {
            Success = success;
        }
        public Result(bool success, string? message, int? statusCode = 200) : this(success)
        {
            Message = message;
            StatusCodes = statusCode;
        }
    }
}

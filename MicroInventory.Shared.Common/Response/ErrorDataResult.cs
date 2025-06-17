namespace MicroInventory.Shared.Common.Response
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data) : base(false, data)
        {
        }

        public ErrorDataResult(T data, int count) : base(false, data, count)
        {
        }

        public ErrorDataResult(string message, T data, int? statusCodes = 200) : base(false, message, data)
        {
            StatusCodes = statusCodes;
        }

        public ErrorDataResult(string message, T data, int count) : base(false, message, data, count)
        {
        }
    }
}

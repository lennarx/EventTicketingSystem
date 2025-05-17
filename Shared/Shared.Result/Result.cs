namespace Shared.Result
{
    public class Result<TValue, TError>
    {
        public readonly TValue? Value;
        public readonly TError? Error;

        private bool _isSuccess;

        private Result(TValue value)
        {
            _isSuccess = true;
            Value = value;
            Error = default;
        }

        private Result(TError error)
        {
            _isSuccess = false;
            Value = default;
            Error = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

        public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);
        public static Result<TValue, TError> Success(TValue value) => new Result<TValue, TError>(value);
        public static Result<TValue, TError> Failure(TError value) => new Result<TValue, TError>(value);

        public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
        {
            if (_isSuccess)
            {
                return success(Value!);
            }
            return failure(Error!);
        }
    }
}

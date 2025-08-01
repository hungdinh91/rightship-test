using OrderService.SharedKernel.Extensions;

namespace OrderService.SharedKernel.Common;


public class Result
{
    public bool IsSuccess { get; }
    public ErrorCode? ErrorCode { get; }
    public object[] MessageParams { get; set; }
    public string? ErrorMessage
    {
        get
        {
            if (ErrorCode != null)
            {
                var description = ErrorCode.GetDescription();
                if (description != null && MessageParams != null && MessageParams.Length > 0)
                {
                    return string.Format(description, MessageParams);
                }

                return description;
            }

            return null;
        }
    }

    protected Result(bool isSuccess, ErrorCode? errorCode, params object[] msgParams)
    {
        if (isSuccess && errorCode != null)
            throw new InvalidOperationException();
        if (!isSuccess && errorCode == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        MessageParams = msgParams;
    }

    public static Result Fail(ErrorCode errorCode, params object[] msgParams)
    {
        return new Result(false, errorCode, msgParams);
    }

    public static Result<T> Fail<T>(ErrorCode errorCode, params object[] msgParams)
    {
        return new Result<T>(false, errorCode, msgParams);
    }

    public static Result Ok()
    {
        return new Result(true, null);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, null);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    public T? Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException();

            return _value;
        }
    }

    protected internal Result(T value, bool isSuccess, ErrorCode? errorCode, params object[] msgParams) : base(isSuccess, errorCode, msgParams)
    {
        _value = value;
    }

    protected internal Result(bool isSuccess, ErrorCode? errorCode, params object[] msgParams) : base(isSuccess, errorCode, msgParams)
    {
    }
}

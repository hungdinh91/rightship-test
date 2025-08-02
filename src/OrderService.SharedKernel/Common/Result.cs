using OrderService.SharedKernel.Extensions;

namespace OrderService.SharedKernel.Common;


public class Result
{
    public bool IsSuccess { get; }
    public ErrorCode? ErrorCode { get; }
    public object[] MessageParams { get; set; }
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_errorMessage))
            {
                return _errorMessage;
            }

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

    protected Result(bool isSuccess, ErrorCode? errorCode, string? errorMessage, params object[] msgParams)
    {
        if (isSuccess && errorCode != null)
            throw new InvalidOperationException();
        if (!isSuccess && errorCode == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        _errorMessage = errorMessage;
        MessageParams = msgParams;
    }

    public static Result Fail(ErrorCode errorCode, params object[] msgParams)
    {
        return new Result(false, errorCode, null, msgParams);
    }

    public static Result<T> Fail<T>(ErrorCode errorCode, params object[] msgParams)
    {
        return new Result<T>(false, errorCode, null, msgParams);
    }

    public static Result Fail(ErrorCode errorCode, string errorMessage)
    {
        return new Result(false, errorCode, errorMessage);
    }

    public static Result<T> Fail<T>(ErrorCode errorCode, string errorMessage)
    {
        return new Result<T>(false, errorCode, errorMessage);
    }

    public static Result Ok()
    {
        return new Result(true, null, null);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, null, null);
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

    protected internal Result(T value, bool isSuccess, ErrorCode? errorCode, string? errorMessage, params object[] msgParams) : base(isSuccess, errorCode, errorMessage, msgParams)
    {
        _value = value;
    }

    protected internal Result(bool isSuccess, ErrorCode? errorCode, string? errorMessage, params object[] msgParams) : base(isSuccess, errorCode, errorMessage, msgParams)
    {
    }
}

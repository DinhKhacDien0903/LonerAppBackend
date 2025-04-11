namespace Loner.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data{get;}
    public string? ErrorMessage { get; }

    private Result(bool isSuccess, T? data, string? ErrorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = ErrorMessage;
    }

    public static Result<T> Success(T data) => new Result<T>(true, data, null);
    public static Result<T> Failure(string ErrorMessage) => new Result<T>(false, default, ErrorMessage);
}
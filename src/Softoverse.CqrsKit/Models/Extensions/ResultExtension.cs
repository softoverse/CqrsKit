namespace Softoverse.CqrsKit.Models.Extensions;

public static class ResultExtension
{
    public static Task<Result> AsTask(this Result result)
    {
        return Task.FromResult(result);
    }

    public static Task<Result<TResponse>> AsTask<TResponse>(this Result<TResponse> result)
    {
        return Task.FromResult(result);
    }
}
namespace Softoverse.CqrsKit.Model;

public class ResultBase
{
    public string? Message { get; set; }
    public bool IsSuccessful { get; set; } = true;
    public IDictionary<string, object>? AdditionalProperties { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}

public static class ResponseDefaults
{
    #region Static Methods

    public static Task<T> DefaultValueResponse<T>(T value) => Task.FromResult(value);

    public static Task<Result> DefaultResponse() => Task.FromResult(Result.Success());

    public static Task<Result<TResponse>> DefaultResponse<TResponse>() => Task.FromResult(Result<TResponse>.Success());

    #endregion Static Methods
}
namespace Softoverse.CqrsKit.Model;

public class ResponseBase
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

    public static Task<Response> DefaultResponse() => Task.FromResult(Response.Success());

    public static Task<Response<TResponse>> DefaultResponse<TResponse>() => Task.FromResult(Response<TResponse>.Success());

    #endregion Static Methods
}
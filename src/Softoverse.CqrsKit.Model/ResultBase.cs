﻿namespace Softoverse.CqrsKit.Model;

public class ResultBase
{
    public string? Message { get; set; }
    public bool IsSuccessful { get; set; } = true;
    public IDictionary<string, object>? AdditionalProperties { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
}

public static class ResultDefaults
{
    public static Task<T> DefaultValueResult<T>(T value) => Task.FromResult(value);

    public static Task<Result> DefaultResult() => Task.FromResult(Result.Success());

    public static Task<Result<TResponse>> DefaultResult<TResponse>() => Task.FromResult(Result<TResponse>.Success());
}
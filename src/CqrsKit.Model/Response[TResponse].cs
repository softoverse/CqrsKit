using CqrsKit.Model.Utility;

using CqrsKit.Model.Extensions;

namespace CqrsKit.Model;

public class Response<TResponse> : Response
{
    private string _errorMessage;
    private string _successMessage;

    public TResponse Payload { get; set; }

    private Response() : this(default!, false, "", new Dictionary<string, object>(), new Dictionary<string, string[]>()) { }

    private Response(TResponse payload = default!, bool isSuccessful = true, string? message = "", IDictionary<string, object>? additionalProperties = null, IDictionary<string, string[]>? errors = null)
    {
        Payload = payload;

        Message = message;
        IsSuccessful = isSuccessful;
        AdditionalProperties = additionalProperties ?? new Dictionary<string, object>();
        Errors = errors ?? new Dictionary<string, string[]>();
    }

    public Response<TResponse> WithPayload(TResponse payload)
    {
        this.Payload = payload;
        return this;
    }

    public new Response<TResponse> WithMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public new Response<TResponse> WithAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        this.AdditionalProperties = additionalProperties;
        return this;
    }

    public new Response<TResponse> AddAdditionalProperty(KeyValuePair<string, object> additionalProperty)
    {
        this.AdditionalProperties?.Add(additionalProperty.Key, additionalProperty.Value);
        return this;
    }

    public new Response<TResponse> AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var additionalProperty in additionalProperties)
        {
            AddAdditionalProperty(additionalProperty);
        }
        return this;
    }

    public new Response<TResponse> WithError(KeyValuePair<string, string[]> error)
    {
        return AddError(error);
    }

    public new Response<TResponse> WithErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public new Response<TResponse> AddError(KeyValuePair<string, string[]> error)
    {
        this.Errors ??= new Dictionary<string, string[]>();
        this.Errors.AddError(error.Key, error.Value);
        return this;
    }

    public new Response<TResponse> AddErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public new Response<TResponse> WithSuccessMessage(string message)
    {
        _successMessage = message;
        return WithMessage(IsSuccessful ? _successMessage : _errorMessage);
    }

    public new Response<TResponse> WithErrorMessage(string message)
    {
        _errorMessage = message;
        return WithMessage(IsSuccessful ? _successMessage : _errorMessage);
    }

    public new static Response<TResponse> Success()
    {
        return Create(true);
    }

    public new static Response<TResponse> Error()
    {
        return Create(false);
    }

    public new static Response<TResponse> Error(CqrsError error)
    {
        return Error().WithMessage(error.Message)
                      .WithErrors(error.Errors);
    }

    public new static Response<TResponse> Create(bool isSuccessful, string? successMessage = null, string? errorMessage = null)
    {
        return Create(isSuccessful, null, default!, null, null)
               .WithSuccessMessage(successMessage!)
               .WithErrorMessage(errorMessage!);
    }

    private static Response<TResponse> Create(bool isSuccessful, string? message, TResponse payload, IDictionary<string, object>? additionalProperties, IDictionary<string, string[]>? errors)
    {
        return new Response<TResponse>(payload, isSuccessful, message, additionalProperties, errors);
    }
}
using Softoverse.CqrsKit.Model.Extensions;

using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Model;

public class Response : ResponseBase
{
    private string _errorMessage;
    private string _successMessage;

    private Response() : this(false, "", new Dictionary<string, object>(), new Dictionary<string, string[]>()) { }

    internal Response(bool isSuccessful = true, string? message = "", IDictionary<string, object>? additionalProperties = null, IDictionary<string, string[]>? errors = null)
    {
        Message = message;
        IsSuccessful = isSuccessful;
        AdditionalProperties = additionalProperties ?? new Dictionary<string, object>();
        Errors = errors ?? new Dictionary<string, string[]>();
    }

    public Response WithMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public Response WithAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        this.AdditionalProperties = additionalProperties;
        return this;
    }

    public Response AddAdditionalProperty(KeyValuePair<string, object> additionalProperty)
    {
        this.AdditionalProperties?.Add(additionalProperty.Key, additionalProperty.Value);
        return this;
    }

    public Response AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var additionalProperty in additionalProperties)
        {
            AddAdditionalProperty(additionalProperty);
        }
        return this;
    }

    public Response WithError(KeyValuePair<string, string[]> error)
    {
        return AddError(error);
    }

    public Response WithErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public Response AddError(KeyValuePair<string, string[]> error)
    {
        this.Errors ??= new Dictionary<string, string[]>();
        this.Errors.AddError(error.Key, error.Value);
        return this;
    }

    public Response AddErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public Response WithSuccessMessage(string message)
    {
        _successMessage = message;
        return WithMessage(IsSuccessful ? _successMessage : _errorMessage);
    }

    public Response WithErrorMessage(string message)
    {
        _errorMessage = message;
        return WithMessage(IsSuccessful ? _successMessage : _errorMessage);
    }

    public static Response Success()
    {
        return Create(true);
    }

    public static Response Error()
    {
        return Create(false);
    }

    public static Response Error(CqrsError error)
    {
        return Error().WithMessage(error.Message)
                      .WithErrors(error.Errors);
    }

    public static Response Create(bool isSuccessful, string? successMessage = null, string? errorMessage = null)
    {
        return Create(isSuccessful, null, null, null)
               .WithSuccessMessage(successMessage!)
               .WithErrorMessage(errorMessage!);
    }

    private static Response Create(bool isSuccessful, string? message, IDictionary<string, object>? additionalProperties, IDictionary<string, string[]>? errors)
    {
        return new Response(isSuccessful, message, additionalProperties, errors);
    }
}
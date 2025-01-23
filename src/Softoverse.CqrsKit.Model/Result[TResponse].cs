﻿using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Model;

public class Result<TResponse> : Result
{
    private bool _isSuccessMessage;
    private string _errorMessage;
    private string _successMessage;

    public TResponse Payload { get; set; }

    private Result() : this(default!, false, "", new Dictionary<string, object>(), new Dictionary<string, string[]>()) { }

    private Result(TResponse payload = default!, bool isSuccessful = true, string? message = null, IDictionary<string, object>? additionalProperties = null, IDictionary<string, string[]>? errors = null)
    {
        Payload = payload;

        Message = message;
        IsSuccessful = isSuccessful;
        AdditionalProperties = additionalProperties;
        Errors = errors;
    }

    public Result<TResponse> WithPayload(TResponse payload)
    {
        this.Payload = payload;
        return this;
    }

    public new Result<TResponse> WithMessage(string message)
    {
        this.Message = message;
        return this;
    }

    public new Result<TResponse> WithAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        this.AdditionalProperties = additionalProperties;
        return this;
    }

    public new Result<TResponse> AddAdditionalProperty(KeyValuePair<string, object> additionalProperty)
    {
        this.AdditionalProperties?.Add(additionalProperty.Key, additionalProperty.Value);
        return this;
    }

    public new Result<TResponse> AddAdditionalProperties(IDictionary<string, object> additionalProperties)
    {
        foreach (var additionalProperty in additionalProperties)
        {
            AddAdditionalProperty(additionalProperty);
        }
        return this;
    }

    public new Result<TResponse> WithError(KeyValuePair<string, string[]> error)
    {
        return AddError(error);
    }

    public new Result<TResponse> WithErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public new Result<TResponse> AddError(KeyValuePair<string, string[]> error)
    {
        this.Errors ??= new Dictionary<string, string[]>();
        this.Errors.AddError(error.Key, error.Value);
        return this;
    }

    public new Result<TResponse> AddErrors(IDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }
        return this;
    }

    public new Result<TResponse> WithMessageLogic(bool logic)
    {
        _isSuccessMessage = logic;
        return WithMessage(_isSuccessMessage ? _successMessage : _errorMessage);;
    }

    public new Result<TResponse> WithSuccessMessage(string message)
    {
        _successMessage = message;
        return WithMessage(_isSuccessMessage ? _successMessage : _errorMessage);
    }

    public new Result<TResponse> WithErrorMessage(string message)
    {
        _errorMessage = message;
        return WithMessage(_isSuccessMessage ? _successMessage : _errorMessage);
    }

    public new static Result<TResponse> Success()
    {
        return Create(true);
    }

    public new static Result<TResponse> Error()
    {
        return Create(false);
    }

    public new static Result<TResponse> Error(CqrsError error)
    {
        return Error().WithMessage(error.Message)
                      .WithErrors(error.Errors);
    }

    public new static Result<TResponse> Create(bool isSuccessful, string? successMessage = null, string? errorMessage = null)
    {
        return Create(isSuccessful, null, default!, null, null)
               .WithSuccessMessage(successMessage!)
               .WithErrorMessage(errorMessage!);
    }

    private static Result<TResponse> Create(bool isSuccessful, string? message, TResponse payload, IDictionary<string, object>? additionalProperties = null, IDictionary<string, string[]>? errors = null)
    {
        return new Result<TResponse>(payload, isSuccessful, message, additionalProperties, errors);
    }
}
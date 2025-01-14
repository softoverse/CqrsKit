namespace Softoverse.CqrsKit.Model.Utility;

public class HandlerStepBase(StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
{
    public StepBehavior Behavior { get; } = behavior;
    public string? Message { get; } = message;
    public bool IsTraversed { get; set; } = false;
}

public class HandlerStep(Func<Task<Response>> @delegate, StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
    : HandlerStepBase(behavior, message)
{
    public Func<Task<Response>> Delegate { get; } = @delegate;
}

public class HandlerStep<TResponse>(Func<Task<Response<TResponse>>> @delegate, StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
    : HandlerStepBase(behavior, message)
{
    public Func<Task<Response<TResponse>>> Delegate { get; } = @delegate;
}
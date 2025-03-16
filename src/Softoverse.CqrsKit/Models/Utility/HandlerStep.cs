namespace Softoverse.CqrsKit.Models.Utility;

public class HandlerStepBase(StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
{
    public StepBehavior Behavior { get; } = behavior;
    public string? Message { get; } = message;
    public bool IsTraversed { get; set; } = false;
}

public class HandlerStep(Func<Task<Result>> @delegate, StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
    : HandlerStepBase(behavior, message)
{
    public Func<Task<Result>> Delegate { get; } = @delegate;
}

public class HandlerStep<TResponse>(Func<Task<Result<TResponse>>> @delegate, StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
    : HandlerStepBase(behavior, message)
{
    public Func<Task<Result<TResponse>>> Delegate { get; } = @delegate;
    
    public static HandlerStep<TResponse> New(Func<Task<Result<TResponse>>> @delegate, StepBehavior behavior = StepBehavior.Mandatory, string? message = null)
    {
        return new HandlerStep<TResponse>(@delegate, behavior, message);
    }
}
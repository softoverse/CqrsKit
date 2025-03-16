using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Executors;

internal static class SequentialStepExecutor
{
    public static async Task<Result<TResponse>> ExecuteStepsAsync<TResponse>(HandlerStep<TResponse>[] steps, CqrsContext context)
    {
        Validate(steps);
        
        Result<TResponse>? finalOutputResponse = null;

        foreach (var step in steps)
        {
            if (step.Behavior == StepBehavior.Skip || step.IsTraversed) continue;

            var result = await step.Delegate();
            context.Result = result;

            if (result.IsSuccess)
            {
                if (step.Behavior == StepBehavior.FinalOutput)
                {
                    finalOutputResponse = result;
                }
            }
            else
            {
                if (step.Behavior is StepBehavior.Mandatory or StepBehavior.FinalOutput or StepBehavior.MustCall)
                {
                    var anyMustCallStep = steps.Any(x => x is
                                                    {
                                                        IsTraversed: false,
                                                        Behavior   : StepBehavior.MustCall
                                                    });

                    if (!anyMustCallStep)
                    {
                        return result; // Stop immediately if a mandatory step fails
                    }

                    foreach (var stepItem in steps.Where(x => x.Behavior != StepBehavior.MustCall))
                    {
                        stepItem.IsTraversed = true;
                    }

                    finalOutputResponse = result;
                }
            }
            
            step.IsTraversed = true;
        }

        // Return the final output response if available; otherwise, a generic success response
        return finalOutputResponse ?? Result<TResponse>.Error();
        

        // return await ExecuteStepsCoreAsync(context,
        //                                    steps,
        //                                    step => step.Delegate(),
        //                                    result => Result<TResponse>.Error().WithMessage(result.Message!),
        //                                    Result<TResponse>.Error);
    }

    private static async Task<TResponse> ExecuteStepsCoreAsync<TStep, TResponse>(CqrsContext context,
                                                                                 TStep[] steps,
                                                                                 Func<TStep, Task<TResponse>> stepDelegate,
                                                                                 Func<TResponse, TResponse> createErrorResponse,
                                                                                 Func<TResponse> createDefaultErrorResponse)
        where TStep : HandlerStepBase
        where TResponse : Result
    {
        TResponse? finalOutputResponse = null;

        foreach (var step in steps)
        {

            Console.WriteLine(stepDelegate.Method.Name);
            
            if (step.Behavior == StepBehavior.Skip || step.IsTraversed) continue;

            var result = await stepDelegate(step);
            context.Result = result;

            if (result.IsSuccess)
            {
                if (step.Behavior == StepBehavior.FinalOutput)
                {
                    finalOutputResponse = result;
                }
            }
            else
            {
                if (step.Behavior is StepBehavior.Mandatory or StepBehavior.FinalOutput)
                {
                    var anyMustCallStep = steps.Any(x => x is
                                                    {
                                                        IsTraversed: false,
                                                        Behavior   : StepBehavior.MustCall
                                                    });

                    if (!anyMustCallStep)
                    {
                        return createErrorResponse(result); // Stop immediately if a mandatory step fails
                    }

                    foreach (var stepItem in steps.Where(x => x.Behavior != StepBehavior.MustCall))
                    {
                        stepItem.IsTraversed = true;
                    }

                    finalOutputResponse = createErrorResponse(result);
                }
            }
            
            step.IsTraversed = true;
        }

        // Return the final output response if available; otherwise, a generic success response
        return finalOutputResponse ?? createDefaultErrorResponse();
    }

    private static bool Validate<TResponse>(HandlerStep<TResponse>[] steps)
    {
        return ThrowIfFinalOutputStepMissing(steps);
    }

    // [Obsolete("Use other overwrite instead", true)]
    // private static bool Validate(HandlerStep[] steps)
    // {
    //     return ThrowIfFinalOutputStepMissing(steps);
    // }

    private static bool ThrowIfFinalOutputStepMissing(HandlerStepBase[] steps)
    {
        if (steps.All(x => x.Behavior != StepBehavior.FinalOutput))
        {
#if DEBUG
            throw new Exception(SequentialStepExecutorErrors.FinalOutputStepMissing().Message);
#else
            return false;
#endif
        }

        return true;
    }
}
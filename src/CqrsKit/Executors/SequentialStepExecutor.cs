using CqrsKit.Model;
using CqrsKit.Model.Errors;
using CqrsKit.Model.Utility;

namespace CqrsKit.Executors;

internal static class SequentialStepExecutor
{
    public static async Task<Response<TResponse>> ExecuteStepsAsync<TResponse>(HandlerStep<TResponse>[] steps, CqrsContext context)
    {
        Validate(steps);

        return await ExecuteStepsCoreAsync(context,
                                           steps,
                                           step => step.Delegate(),
                                           result => Response<TResponse>.Error().WithMessage(result.Message!),
                                           Response<TResponse>.Error);
    }

    private static async Task<TResponse> ExecuteStepsCoreAsync<TStep, TResponse>(CqrsContext context,
                                                                                 TStep[] steps,
                                                                                 Func<TStep, Task<TResponse>> stepDelegate,
                                                                                 Func<TResponse, TResponse> createErrorResponse,
                                                                                 Func<TResponse> createDefaultErrorResponse)
        where TStep : HandlerStepBase
        where TResponse : Response
    {
        TResponse? finalOutputResponse = null;

        foreach (var step in steps)
        {
            step.IsTraversed = true;
            if (step.Behavior == StepBehavior.Skip) continue;

            var result = await stepDelegate(step);
            context.Response = result;

            if (result.IsSuccessful)
            {
                if (step.Behavior == StepBehavior.FinalOutput)
                {
                    finalOutputResponse = result;
                }
            }
            else
            {
                switch (step.Behavior)
                {
                    case StepBehavior.Mandatory:
                    case StepBehavior.FinalOutput:
                        if (steps.Any(x => x is
                                      {
                                          IsTraversed: false,
                                          Behavior   : StepBehavior.MustCall
                                      }))
                        {
                            finalOutputResponse = createErrorResponse(result);
                            continue;
                        }
                        else
                        {
                            return createErrorResponse(result); // Stop immediately if a mandatory step fails
                        }
                    case StepBehavior.Optional:
                    case StepBehavior.Skip:
                    default:
                        continue; // Skip to the next step
                }
            }
        }

        // Return the final output response if available; otherwise, a generic success response
        return finalOutputResponse ?? createDefaultErrorResponse();
    }

    private static bool Validate<TResponse>(HandlerStep<TResponse>[] steps)
    {
        return ThrowIfFinalOutputStepMissing(steps);
    }

    private static bool Validate(HandlerStep[] steps)
    {
        return ThrowIfFinalOutputStepMissing(steps);
    }

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
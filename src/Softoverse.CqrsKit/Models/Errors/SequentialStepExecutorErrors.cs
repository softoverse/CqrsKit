using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Models.Errors;

public static class SequentialStepExecutorErrors
{
    public static CqrsError FinalOutputStepMissing() => CqrsError.Create("SequentialStepExecutor.FinalOutputStepMissing",
                                                                         "The final output step is missing.");
}
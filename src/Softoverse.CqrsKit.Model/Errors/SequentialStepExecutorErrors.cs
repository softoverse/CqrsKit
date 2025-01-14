using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Model.Errors;

public static class SequentialStepExecutorErrors
{
    public static CqrsError FinalOutputStepMissing() => CqrsError.Create("SequentialStepExecutor.FinalOutputStepMissing",
                                                                         "The final output step is missing.");
}
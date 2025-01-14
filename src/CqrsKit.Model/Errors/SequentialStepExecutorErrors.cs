using CqrsKit.Model.Utility;

namespace CqrsKit.Model.Errors;

public static class SequentialStepExecutorErrors
{
    public static CqrsError FinalOutputStepMissing() => CqrsError.Create("SequentialStepExecutor.FinalOutputStepMissing",
                                                                         "The final output step is missing.");
}
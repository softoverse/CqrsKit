namespace Softoverse.CqrsKit.Model.Utility;

public enum StepBehavior
{
    Skip, // The step is skipped
    Mandatory, // The step must succeed; stop execution if it fails
    Optional, // The step can fail; execution continues
    FinalOutput, // The step's output is considered as the final result if it succeeds
    MustCall, // The step must be called event if FinalOutput is unsuccessful
}
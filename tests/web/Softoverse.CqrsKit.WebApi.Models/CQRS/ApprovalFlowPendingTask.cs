using System.ComponentModel.DataAnnotations;

using Softoverse.CqrsKit.Models.Entity;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS;

public class ApprovalFlowPendingTask : BaseApprovalFlowPendingTask
{
    [Range(0, int.MaxValue)]
    public int CompletedStepNumber { get; set; }

    public string? ReviewerUsername { get; set; }

    public string? ApprovalFlowDetailsPageUrl { get; set; }
    public string? ApprovalFlowCorrectionPageUrl { get; set; }

    public bool IsCorrectionAllowed { get; set; }
}
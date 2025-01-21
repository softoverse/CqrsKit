using System.ComponentModel.DataAnnotations;

using Softoverse.CqrsKit.WebApi.Models.CQRS.Custom;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

public class CommandApprovalFlowConfiguration
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long ApprovalFlowConfigurationId { get; set; }

    public ApprovalFlowConfiguration? ApprovalFlowConfiguration { get; set; }

    [Required]
    public long CommandId { get; set; }

    public CommandQuery? Command { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

public class CommandApprovalFlowConfiguration
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long ApprovalFlowConfigurationId { get; set; }

    public CommandApprovalFlowConfiguration? ApprovalFlowConfiguration { get; set; }
    
    public List<CommandApprovalFlowConfiguration> CommandApprovalFlowConfigurations { get; set; } = new();

    [Required]
    public long CommandId { get; set; }

    public CommandQuery? Command { get; set; }
}
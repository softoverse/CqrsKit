using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS;

public class CommandQuery: BaseCommandQuery
{
    public List<CommandQueryUserGroup>? CommandQueryUserGroups { get; set; }
    public CommandApprovalFlowConfiguration? CommandApprovalFlowConfiguration { get; set; } 
}
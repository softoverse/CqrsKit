using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS.Custom;

public class ApprovalFlowConfiguration
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public ApprovalFlowType Type { get; set; }

    public List<ApprovalFlowConfigurationStep> ApprovalFlowConfigurationSteps { get; set; } = new();

    public List<CommandApprovalFlowConfiguration> CommandApprovalFlowConfigurations { get; set; } = new();

    [NotMapped]
    private string _normalizedName { get; set; }

    [NotMapped]
    public string? NormalizedName
    {
        get { return _normalizedName; }
        set
        {
            _normalizedName = Name.Trim().Replace(" ", "");
            value = _normalizedName;
        }
    }
}

public enum ApprovalFlowType
{
    Single = 0,
    Multilevel = 1,
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Softoverse.CqrsKit.WebApi.Models.User;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS.Custom;

public class ApprovalFlowConfigurationStep
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// <para>If ApprovalFlowConfiguration type is Single, then it will be 1</para>
    /// <para>If ApprovalFlowConfiguration type is Multilevel, then it will be 1 and incremental</para>
    /// </summary>
    [Range(1, int.MaxValue)]
    public int SerialNumber { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public long ApprovalFlowConfigurationId { get; set; }
    public ApprovalFlowConfiguration? ApprovalFlowConfiguration { get; set; }

    public long UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }

    public bool IsSelectReviewer { get; set; } = false;
    public bool IsCorrectionAllowed { get; set; } = false;

    [NotMapped]
    private string? _normalizedName { get; set; }

    [NotMapped]
    public string? NormalizedName
    {
        get { return _normalizedName; }
        set
        {
            _normalizedName = Name?.Trim().Replace(" ", "");
            value = _normalizedName;
        }
    }
}
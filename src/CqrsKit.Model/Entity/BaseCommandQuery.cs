using System.ComponentModel.DataAnnotations;

namespace CqrsKit.Model.Entity;

public class BaseCommandQuery
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Namespace { get; set; }

    [Required]
    public string FullName { get; set; }

    #region Response

    public string? ResponseName { get; set; }
    public string? ResponseNamespace { get; set; }
    public string? ResponseFullName { get; set; }

    #endregion Response

    public string? Description { get; set; }

    [Required]
    public bool IsCommand { get; set; }

    [Required]
    public bool IsApprovalFlowRequired { get; set; }
}
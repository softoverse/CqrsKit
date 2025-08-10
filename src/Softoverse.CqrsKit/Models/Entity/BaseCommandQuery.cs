using System.ComponentModel.DataAnnotations;

namespace Softoverse.CqrsKit.Models.Entity;

public class BaseCommandQuery : BaseCommandQuery<long>;

public class BaseCommandQuery<TKey> where TKey : IEquatable<TKey>
{
    [Key]
    public TKey Id { get; set; }

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
    public string? Group { get; set; }

    [Required]
    public bool IsCommand { get; set; }

    [Required]
    public bool IsApprovalFlowRequired { get; set; }
}
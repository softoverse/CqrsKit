using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

namespace Softoverse.CqrsKit.WebApi.Models.User;

public class UserGroup
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

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

    //[Required]
    //public bool CanApproveTask { get; set; }

    public List<CommandQueryUserGroup>? CommandQueryUserGroups { get; set; }

    public List<UserGroupUser>? UserGroupUsers { get; set; }
}
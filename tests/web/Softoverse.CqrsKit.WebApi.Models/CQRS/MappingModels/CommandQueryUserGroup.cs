using System.ComponentModel.DataAnnotations;

using Softoverse.CqrsKit.WebApi.Models.User;

namespace Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

public class CommandQueryUserGroup
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long UserGroupId { get; set; }

    public UserGroup? UserGroup { get; set; }

    [Required]
    public long CommandQueryId { get; set; }

    public CommandQuery? CommandQuery { get; set; }
}
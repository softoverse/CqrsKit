using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Softoverse.CqrsKit.WebApi.Models.User;

public class UserGroupUser
{
    [Key]
    public long Id { get; set; }

    public long UserGroupId { get; set; }
    public UserGroup? UserGroup { get; set; }

    public string UserId { get; set; }
    public IdentityUser? User { get; set; }
}
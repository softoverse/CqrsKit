using System.Text.Json.Serialization;

namespace Softoverse.CqrsKit.WebApi.ViewModels;

public class User
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
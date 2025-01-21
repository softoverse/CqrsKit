using System.Text.Json.Serialization;

namespace Softoverse.CqrsKit.WebApi.Models.ViewModels;

public class TokenDetails
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}

public class TokenResponse : TokenDetails
{
    [JsonPropertyName("expires_in")]
    public decimal ExpiresIn { get; set; }

    [JsonPropertyName("refresh_expires_in")]
    public decimal RefreshExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("login")]
    public TokenDetails Login { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
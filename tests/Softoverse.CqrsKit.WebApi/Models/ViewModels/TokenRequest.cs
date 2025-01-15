using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Softoverse.CqrsKit.WebApi.Models.ViewModels
{
    public class TokenRequest
    {
        [FromForm(Name = "username")]
        public string? Username { get; set; }
        
        [FromForm(Name = "password")]
        public string? Password { get; set; }
        
        [FromForm(Name = "refresh_token")]
        public string? RefreshToken { get; set; }

        [FromForm(Name = "scope")]
        public string? Scope { get; set; } = "apiScope";
        
        [FromForm(Name = "client_id")]
        public string? ClientId { get; set; }
        
        [FromForm(Name = "client_secret")]
        public string? ClientSecret { get; set; }
        
        [FromForm(Name = "grant_type")]
        [RegularExpression("^(password|refresh_token)$", ErrorMessage = "Invalid grant type.")]
        public string? GrantType { get; set; } = "password";
    }
}
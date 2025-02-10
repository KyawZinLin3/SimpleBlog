using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using SimpleBlog.UI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace SimpleBlog.UI.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var requestPayload = new
                {
                    Email = username,
                    Password = password
                };

                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("auth/login", requestPayload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Login failed with status code: {StatusCode}", response.StatusCode);
                    return false;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseContent);
                if (!json.RootElement.TryGetProperty("token", out var tokenElement))
                {
                    _logger.LogWarning("Token not found in the response.");
                    return false;
                }

                var token = tokenElement.GetString();
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token is null or empty.");
                    return false;
                }
                
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userId =jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username),
                    new Claim("AuthToken", token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                _logger.LogInformation("User logged in successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user");
                throw new ApplicationException("Error authenticating user", ex);
            }
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("AuthToken")?.Value;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }


}

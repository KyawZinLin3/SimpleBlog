using SimpleBlog.UI.Models;
using SimpleBlog.UI.Models.Auth;
using SimpleBlog.UI.Models.Content;
using System.Security.Claims;
using System.Text.Json;

namespace SimpleBlog.UI.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        public PostService(IHttpClientFactory httpClientFactory,
                           AuthService authService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _authService = authService;
        }

        public async Task<bool> CreatePost(CreateContent post, bool status)
        {
            try
            {
                var userId = _authService.GetUserId();
                var requestPayload = new
                {
                    Title = post.Title,
                    Content = post.Content,
                    Status = status,
                    AuthorId = userId,
                    PostTags = post.TagId.Select(tagId => new { TagId = tagId }).ToList(),
                    MediaItems = new List<object>()
                };
                var json = JsonSerializer.Serialize(requestPayload, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(json);
                var response = await _httpClient.PostAsJsonAsync("Post", requestPayload);
                var content = response.Content;
                Console.WriteLine(content);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error authenticating user", ex);
            }
        }

        public async Task<List<GetContent>> GetPost()
        {
            try
            {
                var response = await _httpClient.GetAsync("Post");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<GetContent>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    List<GetContent> posts = apiResponse.Data;
                    return posts;
                }

                return  new List<GetContent>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching posts from API", ex);
            }
        }
    }
}

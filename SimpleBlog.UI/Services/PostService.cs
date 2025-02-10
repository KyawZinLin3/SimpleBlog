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

                return new List<GetContent>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching posts from API", ex);
            }
        }

        public async Task<List<GetContent>> GetPostByUser(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("Post/GetByUserId");
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _authService.Logout();
                    throw new UnauthorizedAccessException("Unauthorized access - redirecting to login.");
                }
                if (!response.IsSuccessStatusCode)
                {
                    //throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<GetContent>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
               
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    List<GetContent> posts = apiResponse.Data;
                    return posts;
                }

                return new List<GetContent>();
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Unauthorized access - redirecting to login.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching posts from API", ex);
            }
        }

        public async Task<GetContentDetail> GetPostByDetail(int Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Post/{Id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<GetContentDetail>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    GetContentDetail posts = apiResponse.Data;
                    return posts;
                }

                return new GetContentDetail();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching posts from API", ex);
            }
        }

        public async Task<bool> DeletePost(int Id,string token)
        {

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"Post/{Id}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }
                return true;
            }
            catch (Exception ex) { throw new ApplicationException("Error fetching posts from API", ex); }
        }

        public async Task<bool> EditPost(EditContent editContent)
        {
            try
            {
                var userId = _authService.GetUserId();
                var requestPayload = new
                {
                    Id = editContent.Id,
                    Title = editContent.Title,
                    Content = editContent.Content,
                    Status = true,
                    AuthorId = userId,
                    PostTags = editContent.TagId.Select(tagId => new { TagId = tagId }).ToList(),
                    MediaItems = new List<object>()
                };
                var json = JsonSerializer.Serialize(requestPayload, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(json);
                var response = await _httpClient.PutAsJsonAsync("Post", requestPayload);
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
    }
}

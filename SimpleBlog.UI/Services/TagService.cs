using SimpleBlog.UI.Models;
using SimpleBlog.UI.Models.Tag;
using System.Text.Json;

namespace SimpleBlog.UI.Services
{
    public class TagService
    {
        private readonly HttpClient _httpClient;
        public TagService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<GetTag>> GetTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("tag");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API call failed with status {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<GetTag>>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    List<GetTag> tags = apiResponse.Data;
                    return tags;
                }
                return new List<GetTag>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching tags from API", ex);
            }
        }
    }
}

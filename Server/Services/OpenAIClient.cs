using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;

namespace Server.Services
{
    public class OpenAIClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> SendPrompt(string prompt, string model)
        {
            var requestBody = new
            {
                prompt = prompt,
                model = "text-davinci-003",
                max_tokens = 150,
                temperature = 0.5
            };

            var response = await _httpClient.PostAsJsonAsync("completions", requestBody);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}

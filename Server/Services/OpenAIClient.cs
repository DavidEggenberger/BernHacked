using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Server.Services
{
    public class OpenAIClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIClient(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> SendPrompt(string prompt)
        {
            var requestBody = new
            {
                prompt = $"Sie sind ein Psychologe und beaftragt sich um den Patienten zu kümmern. " +
                $"Bitte antworten sie in kurzen und prägnanten Sätzen. Falls möglich" +
                $"können sie dem Patienten eine Atemübung empfehlen." +
                $"wenn sie sich nicht 100% sicher sind, dann beenden sie bitte die Konversation" +
                $"indem sie Ihre Unsicherheit dem Patienten mitteilen"+
                $"Der Patiente sagt; {prompt}",
                model = "gpt-3.5-turbo",
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services.AzureSpeech;
using Shared;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureSpeechController : ControllerBase
    {
        private readonly AzureSpeechAnalysisAPIClient azureSpeechAnalysisAPIClient;

        public AzureSpeechController(AzureSpeechAnalysisAPIClient azureSpeechAnalysisAPIClient)
        {
            this.azureSpeechAnalysisAPIClient = azureSpeechAnalysisAPIClient;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<AzureCognitiveServicesTokenDTO> GetTokenAsync()
        {
            var token = await azureSpeechAnalysisAPIClient.GetTokenAsync();
            return new AzureCognitiveServicesTokenDTO
            {
                Token = token,
            };
        }
    }
}

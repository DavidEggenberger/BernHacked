using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Services.AzureSpeech
{
    public class AzureSpeechToTextService
    {
        private readonly AzureSpeechAnalysisOptions options;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AzureSpeechToTextService(IOptions<AzureSpeechAnalysisOptions> options, IWebHostEnvironment webHostEnvironment)
        {
            this.options = options.Value;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> AnalyzeBase64(string base64)
        {
            var speechConfig = SpeechConfig.FromSubscription(options.APIKey, "switzerlandnorth");
            speechConfig.SpeechRecognitionLanguage = "de-CH";

            var id = Guid.NewGuid();
            var generatedFileName = $@"{webHostEnvironment.WebRootPath}/{id}.wav";

            var bytes = Convert.FromBase64String(base64);

            var ms = new MemoryStream(bytes);
            var rs = new RawSourceWaveStream(ms, new WaveFormat());
            
            WaveFileWriter.CreateWaveFile(generatedFileName, rs);

            SpeechRecognitionResult speechRecognitionResult;
            using (var audioConfig = AudioConfig.FromWavFileInput(generatedFileName))
            {
                using (var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig))
                {
                    speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                };
            };

            if (File.Exists(generatedFileName))
            {
                File.Delete(generatedFileName);
            }

            return speechRecognitionResult.Text;
        }
    }
}

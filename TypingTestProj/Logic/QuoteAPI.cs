using System.Net;
using Newtonsoft.Json;
using TypingTest_Project.Models;

namespace Typing_Test_Project.Logic
{
    public class QuoteAPI
    {
        private const string ApiUrl = "https://api.quotable.io/random";
        private readonly HttpClient _httpClient;

        public QuoteAPI()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => { return true; };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "TypingTestApp/Project");
        }

        public async Task<bool> InternetCheckAsync()
        {
            try
            {
                using (var response = await _httpClient.GetAsync("http://clients3.google.com/generate_204", HttpCompletionOption.ResponseHeadersRead))
                {
                    return response.IsSuccessStatusCode;
                }
            }
            catch { return false; }
        }
        public async Task<LevelData> GetQuoteFromApiAsync(int levelNumber, HashSet<string> usedIdsInSession)
        {
            int maxRetries = 3;
            QuotableResponse lastData = null;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    string jsonResponse = await _httpClient.GetStringAsync(ApiUrl);
                    var apiData = JsonConvert.DeserializeObject<QuotableResponse>(jsonResponse);

                    if (usedIdsInSession.Contains(apiData._id)) continue;

                    lastData = apiData;

                    return new LevelData
                    {
                        Id = apiData._id,
                        Content = apiData.content,
                        AuthorOrDescription = apiData.author,
                        LevelNumber = levelNumber,
                        Type = LevelType.Quote
                    };
                }
                catch (Exception ex)
                {
                    // If it fails, we let the Engine handle falling back to offline mode
                    throw new Exception($"API Error: {ex.Message}\n\nInner: {ex.InnerException?.Message}");
                }
            }
            if (lastData != null)
            {
                return new LevelData
                {
                    Id = lastData._id,
                    Content = lastData.content,
                    AuthorOrDescription = lastData.author,
                    LevelNumber = levelNumber,
                    Type = LevelType.Quote
                };
            }
            else return null; 
        }
    }
}

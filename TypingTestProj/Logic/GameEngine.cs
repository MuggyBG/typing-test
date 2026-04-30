using System.Net;
using Newtonsoft.Json;
using TypingTest_Project.Models;

namespace TypingTest_Project.Logic
{
    public class GameEngine
    {
        private const string ApiUrl = "https://api.quotable.io/random";
        private const string DataDirName = "Data";
        private const string HistoryFileName = "history_cache.json";
        private const string CodeLevelsFileName = "code_levels.json";

        private readonly HttpClient _httpClient;
        private List<LevelData> codeLevelsCache;
        private HashSet<string> usedIdsInSession;
        private readonly Random _random = new Random();

        private readonly string basePath;
        private readonly string dataDir;
        private readonly string historyPath;
        private readonly string codeLevelsPath;

        public GameEngine()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "TypingTestApp/Project");
            usedIdsInSession = new HashSet<string>();
            basePath = AppDomain.CurrentDomain.BaseDirectory;
            dataDir = Path.Combine(basePath, DataDirName);
            historyPath = ResolveFilePath(HistoryFileName);
            codeLevelsPath = ResolveFilePath(CodeLevelsFileName);

            LoadCodeLevels();
        }
        private async Task<bool> InternetCheck()
        {
            try
            {
                //using (var client = new HttpClient())
                //using (client.OpenRead("http://clients3.google.com/generate_204"))
                using (var response = await _httpClient.GetAsync("http://clients3.google.com/generate_204", HttpCompletionOption.ResponseHeadersRead))
                {
                    return response.IsSuccessStatusCode;
                }
            }
            catch { return false; }
        }

        private string ResolveFilePath(string fileName)
        {
            string inData = Path.Combine(dataDir, fileName);
            if (File.Exists(inData)) return inData;

            string inRoot = Path.Combine(basePath, fileName);
            return inRoot;
        }
        public async Task<LevelData> GetLevelAsync(int levelNumber, LevelType type)
        {
            if (type == LevelType.CodeSnippet)
            {
                return GetCodeLevel(levelNumber);
            }
            else
                if (await InternetCheck())
                {
                    return await GetQuoteFromApiAsync(levelNumber);
                }
            else            {
                return GetQuoteFromOfflineCache(levelNumber);
                }
        }
        private void LoadCodeLevels()
        {
            if (File.Exists(codeLevelsPath))
            {
                try
                {
                    string json = File.ReadAllText(codeLevelsPath);
                    codeLevelsCache = JsonConvert.DeserializeObject<List<LevelData>>(json);
                }
                catch
                {
                    codeLevelsCache = new List<LevelData>();
                }
            }
            else
            {
                codeLevelsCache = new List<LevelData>();
            }
        }

        private LevelData GetCodeLevel(int levelNumber)
        {
            var level = codeLevelsCache.FirstOrDefault(x => x.LevelNumber == levelNumber);

            if (level != null)
            {
                level.Type = LevelType.CodeSnippet;
                return level;
            }
            if (codeLevelsCache != null && codeLevelsCache.Count > 0)
            {
                var randomLevel = codeLevelsCache[_random.Next(codeLevelsCache.Count)];
                return new LevelData
                {
                    Id = randomLevel.Id,
                    Content = randomLevel.Content,
                    AuthorOrDescription = randomLevel.AuthorOrDescription,
                    LevelNumber = levelNumber,
                    Type = LevelType.CodeSnippet
                };
            }

            return new LevelData
            {
                Content = "Error: code_levels.json not found or empty! Please contact the developer.",
                AuthorOrDescription = "Mugetsu",
                Type = LevelType.CodeSnippet
            };
        }

        private async Task<LevelData> GetQuoteFromApiAsync(int levelNumber)
        {
            try
            {
                string jsonResponse = await _httpClient.GetStringAsync(ApiUrl);
                var apiData = JsonConvert.DeserializeObject<QuotableResponse>(jsonResponse);

                LevelData newLevel = new LevelData
                {
                    Id = apiData._id,
                    Content = apiData.content,
                    AuthorOrDescription = apiData.author,
                    LevelNumber = levelNumber,
                    Type = LevelType.Quote
                };
                SaveToHistoryCache(newLevel);
                return newLevel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API Error: {ex.Message}\n\nInner: {ex.InnerException?.Message}", "Debug Info");
                return GetQuoteFromOfflineCache(levelNumber);
            }
        }

        private LevelData GetQuoteFromOfflineCache(int levelNumber)
        {
            if (!File.Exists(historyPath))
            {
                throw new Exception("No internet connection and no offline cache found.\nPlease connect to the internet for the first run.");
            }
            string json = File.ReadAllText(historyPath);
            var history = JsonConvert.DeserializeObject<List<LevelData>>(json);
            if (history == null || history.Count == 0)
            {
                throw new Exception("Offline cache is empty.");
            }
            var availableLevels = history.Where(x => !usedIdsInSession.Contains(x.Id)).ToList();
            if (availableLevels.Count == 0)
            {
                usedIdsInSession.Clear();
                availableLevels = history;
            }
            var selected = availableLevels[_random.Next(availableLevels.Count)];
            usedIdsInSession.Add(selected.Id);
            selected.LevelNumber = levelNumber; 
            return selected;
        }


        public void SaveToHistoryCache(LevelData data)
        {
            var history = new List<LevelData>();
            if (File.Exists(historyPath))
            {
                string json = File.ReadAllText(historyPath);
                history = JsonConvert.DeserializeObject<List<LevelData>>(json) ?? new();
            }

            if (!history.Any(x => x.Id == data.Id))
            {
                history.Add(data);
                File.WriteAllText(historyPath, JsonConvert.SerializeObject(history, Formatting.Indented));
            }
        }
    }
}
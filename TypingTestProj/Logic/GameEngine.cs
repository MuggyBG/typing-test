using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private List<LevelData> _codeLevelsCache;
        private List<string> _usedIdsInSession;

        private readonly string _basePath;
        private readonly string _dataDir;
        private readonly string _historyPath;
        private readonly string _codeLevelsPath;

        public GameEngine()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            _httpClient = new HttpClient(handler);

            _usedIdsInSession = new List<string>();

            _basePath = AppDomain.CurrentDomain.BaseDirectory;
            _dataDir = Path.Combine(_basePath, DataDirName);
            _historyPath = ResolveFilePath(HistoryFileName);
            _codeLevelsPath = ResolveFilePath(CodeLevelsFileName);

            LoadCodeLevels();
        }

        private string ResolveFilePath(string fileName)
        {
            string inData = Path.Combine(_dataDir, fileName);
            if (File.Exists(inData)) return inData;

            string inRoot = Path.Combine(_basePath, fileName);
            return inRoot;
        }
        public async Task<LevelData> GetLevelAsync(int levelNumber, LevelType type)
        {
            if (type == LevelType.CodeSnippet)
            {
                return GetCodeLevel(levelNumber);
            }
            else
            {
                if (InternetCheck()) 
                {
                    return await GetQuoteFromApiAsync(levelNumber);
                }
                else
                {
                    return GetQuoteFromOfflineCache(levelNumber);
                }
            }
        }
        private bool InternetCheck()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch { return false; }
        }
        private void LoadCodeLevels()
        {
            if (File.Exists(_codeLevelsPath))
            {
                try
                {
                    string json = File.ReadAllText(_codeLevelsPath);
                    _codeLevelsCache = JsonConvert.DeserializeObject<List<LevelData>>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"JSON Error: {ex.Message}");
                    _codeLevelsCache = new List<LevelData>();
                }
            }
            else
            {
                MessageBox.Show($"File not found at: {_codeLevelsPath}");
                _codeLevelsCache = new List<LevelData>();
            }
        }

        private LevelData GetCodeLevel(int levelNumber)
        {
            var level = _codeLevelsCache.FirstOrDefault(x => x.LevelNumber == levelNumber);

            if (level != null)
            {
                level.Type = LevelType.CodeSnippet;
                return level;
            }

            // za niva nad dostupnite, vrushtame random
            if (_codeLevelsCache != null && _codeLevelsCache.Count > 0)
            {
                var randomLevel = _codeLevelsCache[new Random().Next(_codeLevelsCache.Count)];
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
            catch (Exception)
            {
                return GetQuoteFromOfflineCache(levelNumber);
            }
        }

        private LevelData GetQuoteFromOfflineCache(int levelNumber)
        {
            if (!File.Exists(_historyPath))
            {
                throw new Exception("No internet connection and no offline cache found.\nPlease connect to the internet for the first run.");
            }
            string json = File.ReadAllText(_historyPath);
            var history = JsonConvert.DeserializeObject<List<LevelData>>(json);
            if (history == null || history.Count == 0)
            {
                throw new Exception("Offline cache is empty.");
            }
            var availableLevels = history.Where(x => !_usedIdsInSession.Contains(x.Id)).ToList();
            if (availableLevels.Count == 0)
            {
                _usedIdsInSession.Clear();
                availableLevels = history;
            }
            Random rnd = new Random();
            var selected = availableLevels[rnd.Next(availableLevels.Count)];
            _usedIdsInSession.Add(selected.Id);
            selected.LevelNumber = levelNumber; 
            return selected;
        }

        private void SaveToHistoryCache(LevelData data)
        {
            try
            {
                List<LevelData> history;
                Directory.CreateDirectory(_dataDir);
                if (File.Exists(_historyPath))
                {
                    string json = File.ReadAllText(_historyPath);
                    history = JsonConvert.DeserializeObject<List<LevelData>>(json) ?? new List<LevelData>();
                }
                else
                {
                    history = new List<LevelData>();
                }

                if (!history.Any(x => x.Id == data.Id))
                {
                    data.LevelNumber = 0;
                    history.Add(data);
                    var writePath = Path.Combine(_dataDir, HistoryFileName);
                    File.WriteAllText(writePath, JsonConvert.SerializeObject(history, Formatting.Indented));
                }
            }
            catch { }
        }
    }
}
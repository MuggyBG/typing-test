using Newtonsoft.Json;
using TypingTest_Project.Models;
namespace TypingTest_Project.Logic
{
    public class Storage
    {
        private const string DataDirName = "Data";
        private const string ScoresFileName = "scores.json";
        private const string HistoryFileName = "history_cache.json";
        private const string CodeLevelsFileName = "code_levels.json";

        private readonly string basePath;
        private readonly string dataDir;
        private readonly string scoresPath;
        private readonly string historyPath;
        private readonly string codeLevelsPath;

        private List<LevelData> codeLevelsCache;
        private readonly Random _random = new Random();
        public Storage()
        {
            basePath = AppDomain.CurrentDomain.BaseDirectory;
            dataDir = Path.Combine(basePath, DataDirName);

            Directory.CreateDirectory(dataDir);

            scoresPath = Path.Combine(dataDir, ScoresFileName);
            historyPath = ResolveFilePath(HistoryFileName);
            codeLevelsPath = ResolveFilePath(CodeLevelsFileName);
            
            LoadCodeLevels();
        }

        private string ResolveFilePath(string fileName)
        {
            string inData = Path.Combine(dataDir, fileName);
            if (File.Exists(inData)) return inData;

            return Path.Combine(basePath, fileName);
        }
        public List<ScoreEntry> LoadScores()
        {
            try
            {
                if (!File.Exists(scoresPath))
                {
                    return new List<ScoreEntry>();
                }

                string json = File.ReadAllText(scoresPath);
                var list = JsonConvert.DeserializeObject<List<ScoreEntry>>(json);
                return list ?? new List<ScoreEntry>();
            }
            catch
            {
                return new List<ScoreEntry>();
            }
        }
        public void SaveScore(ScoreEntry entry)
        {
            try
            {
                var scores = LoadScores();
                scores.Add(entry);
                string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
                File.WriteAllText(scoresPath, json);
            }
            catch { }
        }
        private void LoadCodeLevels()
        {
            if (File.Exists(codeLevelsPath))
            {
                try
                {
                    string json = File.ReadAllText(codeLevelsPath);
                    codeLevelsCache = JsonConvert.DeserializeObject<List<LevelData>>(json) ?? new List<LevelData>();
                }
                catch { codeLevelsCache = new List<LevelData>(); }
            }
            else
            {
                codeLevelsCache = new List<LevelData>();
            }
        }
        public LevelData GetCodeLevel(int levelNumber)
        {
            var level = codeLevelsCache.FirstOrDefault(x => x.LevelNumber == levelNumber);
            if (level != null)
            {
                level.Type = LevelType.CodeSnippet;
                return level;
            }

            if (codeLevelsCache.Count > 0)
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

        public LevelData GetQuoteFromOfflineCache(int levelNumber, HashSet<string> usedIdsInSession)
        {
            if (!File.Exists(historyPath))
                throw new Exception("No internet connection and no offline cache found.\nPlease connect to the internet for the first run.");

            string json = File.ReadAllText(historyPath);
            var history = JsonConvert.DeserializeObject<List<LevelData>>(json);

            if (history == null || history.Count == 0)
                throw new Exception("Offline cache is empty.");

            var availableLevels = history.Where(x => !usedIdsInSession.Contains(x.Id)).ToList();

            if (availableLevels.Count == 0)
            {
                usedIdsInSession.Clear(); 
                availableLevels = history;
            }

            var selected = availableLevels[_random.Next(availableLevels.Count)];
            selected.LevelNumber = levelNumber;
            return selected;
        }

        public void SaveToHistoryCache(LevelData data)
        {
            var history = new List<LevelData>();
            if (File.Exists(historyPath))
            {
                string json = File.ReadAllText(historyPath);
                history = JsonConvert.DeserializeObject<List<LevelData>>(json) ?? new List<LevelData>();
            }

            if (!history.Any(x => x.Id == data.Id))
            {
                history.Add(data);
                File.WriteAllText(historyPath, JsonConvert.SerializeObject(history, Formatting.Indented));
            }
        }
    }
}


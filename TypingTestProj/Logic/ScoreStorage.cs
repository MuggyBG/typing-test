using Newtonsoft.Json;
using TypingTest_Project.Models;

namespace TypingTest_Project.Logic
{
    public class ScoreStorage
    {
        private const string DataDirName = "Data";
        private const string ScoresFileName = "scores.json";

        private readonly string _dataDir;
        private readonly string _scoresPath;

        public ScoreStorage()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _dataDir = Path.Combine(basePath, DataDirName);
            _scoresPath = Path.Combine(_dataDir, ScoresFileName);
        }

        public List<ScoreEntry> LoadScores()
        {
            try
            {
                if (!File.Exists(_scoresPath))
                {
                    return new List<ScoreEntry>();
                }

                string json = File.ReadAllText(_scoresPath);
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
                Directory.CreateDirectory(_dataDir);
                var scores = LoadScores();
                scores.Add(entry);
                string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
                string writePath = Path.Combine(_dataDir, ScoresFileName);
                File.WriteAllText(writePath, json);
            }
            catch { }
        }
    }
}


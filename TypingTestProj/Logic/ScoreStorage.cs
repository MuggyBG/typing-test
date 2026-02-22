using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TypingTest_Project.Models;

namespace TypingTest_Project.Logic
{
    public class ScoreStorage
    {
        private const string DataDirName = "Data";
        private const string ScoresFileName = "scores.json";

        private readonly string _basePath;
        private readonly string _dataDir;
        private readonly string _scoresPath;

        public ScoreStorage()
        {
            _basePath = AppDomain.CurrentDomain.BaseDirectory;
            _dataDir = Path.Combine(_basePath, DataDirName);
            _scoresPath = ResolveFilePath(ScoresFileName);
        }

        private string ResolveFilePath(string fileName)
        {
            string inData = Path.Combine(_dataDir, fileName);
            if (File.Exists(inData)) return inData;

            string inRoot = Path.Combine(_basePath, fileName);
            return inRoot;
        }
        public IList<ScoreEntry> LoadScores()
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
                var scores = LoadScores().ToList();
                scores.Add(entry);
                string json = JsonConvert.SerializeObject(scores, Formatting.Indented);
                string writePath = Path.Combine(_dataDir, ScoresFileName);
                File.WriteAllText(writePath, json);
            }
            catch { }
        }
    }
}


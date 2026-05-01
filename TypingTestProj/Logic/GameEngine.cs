using System.Net;
using Newtonsoft.Json;
using Typing_Test_Project.Logic;
using TypingTest_Project.Models;

namespace TypingTest_Project.Logic
{
    public class GameEngine
    {
        private readonly QuoteAPI _apiService;
        private readonly Storage _storage;
        private HashSet<string> _usedIdsInSession;
        public GameEngine()
        {
            _apiService = new QuoteAPI();
            _storage = new Storage();
            _usedIdsInSession = new HashSet<string>();
        }
        public async Task<LevelData> GetLevelAsync(int levelNumber, LevelType type)
        {
            if (type == LevelType.CodeSnippet)
            {
                return _storage.GetCodeLevel(levelNumber);
            }
            if (await _apiService.InternetCheckAsync())
            {
                try
                {
                    var newLevel = await _apiService.GetQuoteFromApiAsync(levelNumber, _usedIdsInSession);
                    if (newLevel != null)
                    {
                        _usedIdsInSession.Add(newLevel.Id);
                        _storage.SaveToHistoryCache(newLevel);
                        return newLevel;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Debug Info");

                }
            }
            var offlineLevel = _storage.GetQuoteFromOfflineCache(levelNumber, _usedIdsInSession);
            _usedIdsInSession.Add(offlineLevel.Id);
            return offlineLevel; 
        }
    }
}
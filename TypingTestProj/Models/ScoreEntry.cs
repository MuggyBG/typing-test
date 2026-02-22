using System;
using Newtonsoft.Json;
using TypingTest_Project.Logic;

namespace TypingTest_Project.Models
{
    public class ScoreEntry
    {
        [JsonProperty("mode")]
        public GameMode Mode { get; set; }

        [JsonProperty("level")]
        public int LevelReached { get; set; }

        [JsonProperty("timeSeconds")]
        public int TimeSeconds { get; set; }

        [JsonProperty("wpm")]
        public int Wpm { get; set; }

        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}


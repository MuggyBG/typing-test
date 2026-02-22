using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTest_Project.Models
{
    public enum LevelType
    {
        Quote,
        CodeSnippet
    }
    public class LevelData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("author")]
        public string AuthorOrDescription { get; set; }

        [JsonProperty("level")]
        public int LevelNumber { get; set; }

        public LevelType Type { get; set; }
        

        public bool IsCompleted { get; set; } = false;
    }

}

using System;
using TypingTest_Project.Models;

namespace TypingTest_Project.Logic
{
    public class GameSession
    {
        public int SecondsElapsed { get; set; }
        public int TotalSessionErrors { get; set; }
        public int TotalCharsTyped { get; set; }
        public int CurrentLevelNumber { get; set; }
        public GameMode CurrentMode { get; set; } 

        public GameSession(GameMode mode)
        {
            CurrentMode = mode;
            CurrentLevelNumber = 1;
            SecondsElapsed = 0;
            TotalSessionErrors = 0;
            TotalCharsTyped = 0;
        }

        public int GetCurrentWpm()
        {
            if (SecondsElapsed == 0) return 0;

            // using WPM formula (Characters / 5) / Minutes
            double minutes = SecondsElapsed / 60.0;
            double words = TotalCharsTyped / 5.0;
            return (int)Math.Round(words / minutes);
        }

        public double GetAccuracyPercent()
        {
            if (TotalCharsTyped == 0) return 100;

            double correctChars = TotalCharsTyped - TotalSessionErrors;
            if (correctChars < 0) correctChars = 0;

            return Math.Round((correctChars / TotalCharsTyped) * 100, 2);
        }
        public string BuildStatsSummary(string msg)
        {
            return $"{msg}\nWPM: {GetCurrentWpm()}  |  Accuracy: {GetAccuracyPercent():0.00}%  |  Errors: {TotalSessionErrors}\n Difficulty: {CurrentMode.ToString()}  |  Level Reached: {CurrentLevelNumber}";
        }

        public bool CheckWinCondition()
        {
            if (CurrentMode == GameMode.Standard && CurrentLevelNumber > 50) return true;
            if (CurrentMode == GameMode.CodeOnly && CurrentLevelNumber > 20) return true;

            return false;
        }

        public LevelType DetermineLevelType()
        {
            if (CurrentMode == GameMode.CodeOnly) return LevelType.CodeSnippet;
            return LevelType.Quote;
        }
    }
}
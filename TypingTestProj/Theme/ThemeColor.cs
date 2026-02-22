using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TypingTest_Project.Theme
{
        public static class ThemeColor
        {
            public static readonly Color DarkBackground = Color.FromArgb(30, 30, 30);
            public static readonly Color DarkPanel = Color.FromArgb(45, 45, 48);
            public static readonly Color DarkText = Color.FromArgb(220, 220, 220);
            public static readonly Color DarkAccent = Color.FromArgb(0, 122, 204);


            public static readonly Color LightBackground = Color.FromArgb(211, 211, 211); 
            public static readonly Color LightPanel = Color.FromArgb(238, 232, 213);
            public static readonly Color LightText = Color.FromArgb(30, 30, 30); 
            public static readonly Color LightAccent = Color.FromArgb(38, 139, 210); 

            public static readonly Color ErrorBg = Color.FromArgb(255, 100, 100); 
            public static readonly Color CorrectBg = Color.FromArgb(76, 175, 80); 
            public static readonly Color CorrectText = Color.SeaGreen; 
            public static readonly Color PlaceholderText = Color.FromArgb(80, 80, 80);
        }
}

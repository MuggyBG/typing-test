using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TypingTest_Project.Theme;
using TypingTest_Project;

namespace TypingTest_Project
{
    public partial class ResultForm : Form
    {
        public ResultForm(string summaryText, bool isDarkMode)
        {
            InitializeComponent();
            txtSummary.Text = NormalizeNewlines(summaryText);
            ApplyTheme(isDarkMode);

            btnOk.Click += (_, __) => this.Close();
        }
        private void ApplyTheme(bool isDarkMode)
        {
            var bg = isDarkMode ? ThemeColor.DarkBackground : ThemeColor.LightBackground;
            var panel = isDarkMode ? ThemeColor.DarkPanel : ThemeColor.LightPanel;
            var text = isDarkMode ? ThemeColor.DarkText : ThemeColor.LightText;
            var accent = isDarkMode ? ThemeColor.DarkAccent : ThemeColor.LightAccent;

            BackColor = bg;

            pnlHeader.BackColor = accent;
            lblTitle.ForeColor = Color.White;

            pnlBody.BackColor = panel;
            txtSummary.BackColor = panel;
            txtSummary.ForeColor = text;

            pnlFooter.BackColor = bg;

            btnOk.BackColor = text;
            btnOk.ForeColor = bg;
        }
        private static string NormalizeNewlines(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }
    }
}

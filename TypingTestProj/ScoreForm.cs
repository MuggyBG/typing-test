using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TypingTest_Project.Logic;
using TypingTest_Project.Theme;

namespace TypingTest_Project
{
    public partial class ScoreForm : Form
    {
        private readonly Storage _storage = new Storage();
        public ScoreForm(bool isDarkMode)
        {
            InitializeComponent();
            ApplyTheme(isDarkMode);

            btnClose.Click += (_, __) => this.Close();

            LoadScores();
        }
        private void ApplyTheme(bool isDarkMode)
        {
            Color bg = isDarkMode ? ThemeColor.DarkBackground : ThemeColor.LightBackground;
            Color panel = isDarkMode ? ThemeColor.DarkPanel : ThemeColor.LightPanel;
            Color text = isDarkMode ? ThemeColor.DarkText : ThemeColor.LightText;
            Color accent = isDarkMode ? ThemeColor.DarkAccent : ThemeColor.LightAccent;

            this.BackColor = bg;

            pnlHeader.BackColor = accent;
            lblTitle.ForeColor = Color.White;

            lvwScores.BackColor = panel;
            lvwScores.ForeColor = text;

            pnlFooter.BackColor = bg;

            btnClose.BackColor = text;
            btnClose.ForeColor = bg;
        }
        private void LoadScores()
        {
            lvwScores.Items.Clear();

            try
            {
                var scores = _storage.LoadScores()
                    .OrderByDescending(s => s.Wpm)
                    .ThenByDescending(s => s.Accuracy)
                    .ThenByDescending(s => s.Timestamp)
                    .ToList();

                foreach (var s in scores)
                {
                    var item = new ListViewItem(new[]
                    {
                        s.Wpm.ToString(),
                        $"{s.Accuracy:0.0}%",
                        s.Errors.ToString(),
                        s.LevelReached.ToString(),
                        s.Mode.ToString(),
                        s.TimeSeconds.ToString(),
                        s.Timestamp.ToString("yyyy-MM-dd HH:mm")
                    });
                    lvwScores.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load scores: " + ex.Message);
            }
        }
    }
}

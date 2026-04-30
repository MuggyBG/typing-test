using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Forms; 
using TypingTest_Project.Logic;
using TypingTest_Project.Models;
using TypingTest_Project.Theme;

namespace TypingTest_Project
{
    public enum GameMode
    {
        Standard,   
        Extended,   
        CodeOnly,   
        Endless     
    }
    public partial class MainForm : Form
    {
        private const string startPromptText = "Begin typing to start.";
        private bool startPromptIsShown = false;
        private const int maxLines = 8;
        private const double PreloadNextLevelWhenRemainingFraction = 0.30;
        private sealed record Level(int LevelNumber, string Text);


        private readonly Queue<Level> _pendingLines = new();


        private Level? currentLine = null;
        private LevelData prefetchedLevelData = null;
        private Task<LevelData>? nextLevelTask = null;
        private int totalLinesInCurrentLevel = 0;
        private bool hotkeyLevelSkip = false;
        private bool hotkeyLevelReroll = false;

        private string? completedLinePendingDrop = null;
        private readonly ScoreStorage scoreStorage = new ScoreStorage();

        private bool _isDarkMode = true;
        private GameEngine _engine;
        private LevelData _currentLevel;
        private bool _isGameRunning = false;
        private int _secondsElapsed = 0;
        private int _currentLevelNumber = 1;
        private int _totalSessionErrors = 0;
        private int _totalCharsTyped = 0;
        private int _lastInputLength = 0;
        private int _levelSecondsElapsed = 0;

       
        private GameMode _currentMode = GameMode.Standard;

        public MainForm()
        {
            InitializeComponent();
            _engine = new GameEngine();

            this.Controls.Add(pnlMenu); 
            //this.Controls.Add(pnlGame);

            pnlGame.Visible = false;  
            pnlMenu.Visible = true;   
            pnlMenu.Dock = DockStyle.Fill;
            pnlMenu.BringToFront();   
           
            WireUpNonDesignerEvents();
            SetInputPlaceholder();
            ApplyTheme();
            ShowMenu();
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                e.SuppressKeyPress = true;
                _ = RerollLevelAsync();
                return;
            }

            if (e.KeyCode == Keys.F3)
            {
                e.SuppressKeyPress = true;
                SkipLevelHotkeyNoob();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                FinishViaEscape();
            }
        }

        private void ShowMenu()
        {
            pnlMenu.Visible = true;
            pnlMenu.BringToFront();     
            pnlGame.Visible = false;
            pnlStats.Visible = false;
            btnFinish.Visible = false;
            _isGameRunning = false;
            gameTimer.Stop();

            rtbInput.ReadOnly = true;
            rtbInput.Enabled = true;
            SetInputPlaceholder();
        }

    
        private void btnModeStandard_Click(object sender, EventArgs e) { InitGame(GameMode.Standard); }
        private void btnModeExtended_Click(object sender, EventArgs e) { InitGame(GameMode.Extended); }
        private void btnModeCode_Click(object sender, EventArgs e) { InitGame(GameMode.CodeOnly); } 
        private void btnModeEndless_Click(object sender, EventArgs e) { InitGame(GameMode.Endless); }

        private void InitGame(GameMode mode)
        {
            _currentMode = mode;
            _currentLevelNumber = 1;

            pnlMenu.Visible = false;
            pnlGame.Visible = true;
            pnlStats.Visible = true;
            btnFinish.Visible = true;

            pnlGame.BringToFront();
            rtbInput.Focus();       

            StartGame();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            GameOver("Game finished.");
        }

        private async void StartGame()
        {
            if (_currentLevelNumber == 1)
            {
                _secondsElapsed = 0;
                _totalSessionErrors = 0;
                _totalCharsTyped = 0;
                labLastScore.Text = "";
            }

            if (CheckWinCondition())
            {
                GameOver("Congrats! You completed the mode.");
                return;
            }

            _isGameRunning = true;

            RemoveInputPlaceholder();
            rtbInput.Text = "";
            rtbInput.ReadOnly = false;
            rtbInput.Enabled = true;
            rtbInput.Focus();

            if (_currentLevelNumber == 1) _secondsElapsed = 0;
            _levelSecondsElapsed = 0;
            gameTimer.Stop();

            labLevelInfo.Text = "Loading next level.................";

            try
            {
                LevelType typeToLoad = DetermineLevelType();

                try
                {
                    _currentLevel = await _engine.GetLevelAsync(_currentLevelNumber, typeToLoad);
                }
                catch
                {
                    _currentLevel = await _engine.GetLevelAsync(_currentLevelNumber, LevelType.CodeSnippet);
                    labLevelInfo.Text = "Offline Mode - API Error - Loaded Code Level";
                }

              
                InitializeLevelBuffer(_currentLevelNumber, _currentLevel);
                UpdateTargetDisplayAndHighlight();
                if (!labLevelInfo.Text.Contains("Offline"))
                {
                    if(_currentLevelNumber == 1)
                    {
                        labLevelInfo.Text = $"Level {_currentLevelNumber} ({_currentMode}): {_currentLevel.AuthorOrDescription}. Begin typing to start.";
                    }
                    else labLevelInfo.Text = $"Level {_currentLevelNumber} ({_currentMode}): {_currentLevel.AuthorOrDescription}";
                }

                UpdateStats();
            }
            catch (Exception ex)
            {
                gameTimer.Stop();
                MessageBox.Show("Error: " + ex.Message);
                ShowMenu();
            }
        }

        private void InitializeLevelBuffer(int levelNumber, LevelData level)
        {
            _pendingLines.Clear();
            prefetchedLevelData = null;
            nextLevelTask = null;
            totalLinesInCurrentLevel = 0;
            completedLinePendingDrop = null;

            foreach (var line in SplitIntoLines(level.Content))
            {
                _pendingLines.Enqueue(new Level(levelNumber, line));
                totalLinesInCurrentLevel++;
            }

            currentLine = _pendingLines.Count > 0 ? _pendingLines.Dequeue() : new Level(levelNumber, "");

            RemoveInputPlaceholder();
            rtbInput.Text = "";
            _lastInputLength = 0;
        }

        private static IEnumerable<string> SplitIntoLines(string content)
        {
            return (content ?? string.Empty).Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        }

        private bool CheckWinCondition()
        {
            switch (_currentMode)
            {
                case GameMode.Standard: return _currentLevelNumber > 50;
                case GameMode.Extended: return _currentLevelNumber > 100;
                case GameMode.CodeOnly: return _currentLevelNumber > 75;
                case GameMode.Endless: return false;
                default: return false;
            }
        }

        private LevelType DetermineLevelType()
        {
            switch (_currentMode)
            {
                case GameMode.Standard:
                    return _currentLevelNumber <= 25 ? LevelType.Quote : LevelType.CodeSnippet;
                case GameMode.Extended:
                    return _currentLevelNumber <= 50 ? LevelType.Quote : LevelType.CodeSnippet;
                case GameMode.CodeOnly:
                    return LevelType.CodeSnippet;
                case GameMode.Endless:
                    return (new Random().Next(0, 2) == 0) ? LevelType.Quote : LevelType.CodeSnippet;
                default:
                    return LevelType.Quote;
            }
        }

        private void GameOver(string message)
        {
            gameTimer.Stop();
            _isGameRunning = false;
            rtbInput.ReadOnly = true;
            SaveScoreSnapshot();
            using (var results = new ResultForm(BuildStatsSummary(message), _isDarkMode))
            {
                results.ShowDialog(this);
            }
            ShowMenu();
        }

        private string BuildStatsSummary(string message)
        {
            int wpm = GetCurrentWpm();
            double accuracy = GetAccuracyPercent();

            return
                $"{message}\n\nMode: {_currentMode}\nReached Level: {_currentLevelNumber}\nTotal Time: {_secondsElapsed}s\nWPM: {wpm}\nAccuracy: {accuracy:0.0}%\nErrors: {_totalSessionErrors}";
        }

        private void btnThemeToggle_Click(object sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Color bg = _isDarkMode ? ThemeColor.DarkBackground : ThemeColor.LightBackground;
            Color panel = _isDarkMode ? ThemeColor.DarkPanel : ThemeColor.LightPanel;
            Color text = _isDarkMode ? ThemeColor.DarkText : ThemeColor.LightText;
            Color accent = _isDarkMode ? ThemeColor.DarkAccent : ThemeColor.LightAccent;
            Color targetDefault = GetTargetDefaultTextColor();

            this.BackColor = bg;
            pnlHeader.BackColor = panel;
            pnlStats.BackColor = panel;
            pnlGame.BackColor = bg;
            pnlMenu.BackColor = bg; 
            pnlInfo.BackColor = accent;

            labLevelInfo.ForeColor = Color.White;
            labTitle.ForeColor = text;
            labLevel.ForeColor = text;
            labTimer.ForeColor = text;
            labWPM.ForeColor = _isDarkMode ? Color.LimeGreen : Color.FromArgb(0, 120, 0);
            labAccuracy.ForeColor = _isDarkMode ? Color.Gold : Color.FromArgb(180, 120, 0);
            labLastScore.ForeColor = text;
            labHint.ForeColor = text;

            rtbTarget.BackColor = bg;
            rtbTarget.ForeColor = targetDefault;
            rtbInput.BackColor = bg;
            rtbInput.ForeColor = startPromptIsShown ? ThemeColor.PlaceholderText : text;

            btnThemeToggle.ForeColor = text;
            btnThemeToggle.BackColor = panel;

            btnFinish.BackColor = text;
            btnFinish.ForeColor = bg;
            btnScores.BackColor = text;
            btnScores.ForeColor = bg;
            btnScoreboard2.BackColor = text;
            btnScoreboard2.ForeColor = bg;

            btnModeStandard.BackColor = panel;
            btnModeExtended.BackColor = panel;
            btnModeCode.BackColor = panel;
            btnModeEndless.BackColor = panel;

            btnModeStandard.ForeColor = text;
            btnModeExtended.ForeColor = text;
            btnModeCode.ForeColor = text;
            btnModeEndless.ForeColor = text;
        }

  

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            _secondsElapsed++;
            _levelSecondsElapsed++;
            labTimer.Text = $"Time: {_secondsElapsed}s";

            double minutes = _levelSecondsElapsed / 60.0;
            int chars = rtbInput.Text.Length;
            int wpm = (minutes > 0) ? (int)((chars / 5.0) / minutes) : 0;
            labWPM.Text = $"WPM: {wpm}";
        }

        private void rtbInput_TextChanged(object sender, EventArgs e)
        {
            if (!_isGameRunning) return;
            if (startPromptIsShown) return;
            UpdateTargetDisplayAndHighlight();
            if (currentLine != null)
            {
                var targetLine = currentLine.Text;
                if (rtbInput.Text.Length >= targetLine.Length && targetLine.Length > 0)
                {
                    TryAdvanceLine(forceCorrect: false);
                }
            }
        }

        private void UpdateTargetDisplayAndHighlight()
        {
            if (currentLine == null) return;
            MaybePrefetchNextLevel();
            if (_currentLevel?.Type == LevelType.CodeSnippet && completedLinePendingDrop != null)
            {
                int dropAt = Math.Max(1, currentLine.Text.Length / 2);
                if (rtbInput.Text.Length >= dropAt)
                {
                    completedLinePendingDrop = null;
                }
            }
            var lines = new List<string>(maxLines);
            int highlightOffset = 0;
            if (!string.IsNullOrEmpty(completedLinePendingDrop) && _currentLevel?.Type == LevelType.CodeSnippet)
            {
                lines.Add(completedLinePendingDrop);
                highlightOffset = completedLinePendingDrop.Length + 1; 
            }
            lines.Add(currentLine.Text);
            foreach (var l in _pendingLines.Take(maxLines - lines.Count))
            {
                lines.Add(l.Text);
            }
            string newTargetText = string.Join("\n", lines);
            if (rtbTarget.Text != newTargetText)
            {
                rtbTarget.Text = newTargetText;
            }
            string typed = rtbInput.Text;
            string targetLine = currentLine.Text;
            int max = Math.Min(typed.Length, targetLine.Length);
            SuspendDrawing(rtbTarget);
            int savedStart = rtbTarget.SelectionStart;
            int savedLen = rtbTarget.SelectionLength;

            rtbTarget.SelectAll();
            rtbTarget.SelectionBackColor = rtbTarget.BackColor;
            rtbTarget.SelectionColor = GetTargetDefaultTextColor();
            for (int i = 0; i < max; i++)
            {
                rtbTarget.Select(highlightOffset + i, 1);
                bool ok = typed[i] == targetLine[i];
                rtbTarget.SelectionColor = Color.White;
                rtbTarget.SelectionBackColor = ok ? ThemeColor.CorrectBg : ThemeColor.ErrorBg;
            }
            rtbTarget.Select(savedStart, savedLen);
            ResumeDrawing(rtbTarget);
            _lastInputLength = typed.Length;
            UpdateStats();
        }

        private void TryAdvanceLine(bool forceCorrect)
        {
            if (!_isGameRunning) return;
            if (currentLine == null) return;

            string targetLine = currentLine.Text;
            string typed = forceCorrect ? targetLine : rtbInput.Text;
            _totalSessionErrors += CountLineErrors(typed, targetLine);
            _totalCharsTyped += Math.Min(typed.Length, Math.Max(0, targetLine.Length));

            completedLinePendingDrop = _currentLevel?.Type == LevelType.CodeSnippet ? targetLine : null;

            if (_pendingLines.Count == 0)
            {
                LevelComplete();
                return;
            }

            var next = _pendingLines.Dequeue();
            bool levelChanged = next.LevelNumber != _currentLevelNumber;
            currentLine = next;

            if (levelChanged)
            {
                _currentLevelNumber = next.LevelNumber;
                
                if (prefetchedLevelData != null)
                {
                    _currentLevel = prefetchedLevelData;
                    prefetchedLevelData = null;
                    labLevelInfo.Text = $"Level {_currentLevelNumber} ({_currentMode}): {_currentLevel.AuthorOrDescription}";
                }
                else
                {
                    labLevelInfo.Text = $"Level {_currentLevelNumber} ({_currentMode})";
                }
            }

            rtbInput.Text = "";
            _lastInputLength = 0;
            UpdateTargetDisplayAndHighlight();
        }

        private static int CountLineErrors(string typed, string target)
        {
            typed ??= string.Empty;
            target ??= string.Empty;
            int errors = 0;
            int min = Math.Min(typed.Length, target.Length);
            for (int i = 0; i < min; i++)
            {
                if (typed[i] != target[i]) errors++;
            }
            errors += Math.Abs(typed.Length - target.Length);
            return errors;
        }

        private async void MaybePrefetchNextLevel()
        {
            if (currentLine == null) return;
            if (nextLevelTask != null) return;

            int remainingInCurrentLevel = (currentLine.LevelNumber == _currentLevelNumber ? 1 : 0)
                + _pendingLines.Count(l => l.LevelNumber == _currentLevelNumber);

            int totalInCurrentLevel = Math.Max(1, totalLinesInCurrentLevel);
            double remainingFraction = remainingInCurrentLevel / (double)totalInCurrentLevel;
            if (remainingFraction > PreloadNextLevelWhenRemainingFraction) return;

            int nextLevelNumber = _currentLevelNumber + 1;
            LevelType typeToLoad = DetermineLevelType();

            nextLevelTask = _engine.GetLevelAsync(nextLevelNumber, typeToLoad);
            try
            {
                var nextLevel = await nextLevelTask;
                prefetchedLevelData = nextLevel;
                if (nextLevel.Type == LevelType.CodeSnippet)
                {
                    _pendingLines.Enqueue(new Level(nextLevelNumber, ""));
                }
                foreach (var line in SplitIntoLines(nextLevel.Content))
                {
                    _pendingLines.Enqueue(new Level(nextLevelNumber, line));
                }
            }
            catch { }
        }

        private void SkipLevelHotkeyNoob()  
        {
            if (!_isGameRunning) return;
            if (hotkeyLevelSkip) return;

            hotkeyLevelSkip = true;
            try
            {
                labLastScore.Text = $"Level {_currentLevelNumber} Skipped (F3). GET GOOD?";
                labLastScore.ForeColor = Color.Gold;

                _pendingLines.Clear();
                currentLine = null;
                nextLevelTask = null;
                prefetchedLevelData = null;
                totalLinesInCurrentLevel = 0;
                _lastInputLength = 0;
                rtbInput.Text = "";

                _currentLevelNumber++;
                StartGame();
            }
            finally
            {
                hotkeyLevelSkip = false;
            }
        }

        private void FinishViaEscape()
        {
            if (!_isGameRunning) return;
            GameOver("Game finished (ESC).");
        }

        private async Task RerollLevelAsync()
        {
            if (!_isGameRunning) return;
            if (hotkeyLevelReroll) return;

            hotkeyLevelReroll = true;
            try
            {
                labLastScore.Text = $"Level {_currentLevelNumber}. Rerolled Line";
                labLastScore.ForeColor = Color.Gold;

                _pendingLines.Clear();
                currentLine = null;
                nextLevelTask = null;
                prefetchedLevelData = null;
                totalLinesInCurrentLevel = 0;
                completedLinePendingDrop = null;
                _lastInputLength = 0;
                rtbInput.Text = "";

                LevelType typeToLoad = DetermineLevelType();
                _currentLevel = await _engine.GetLevelAsync(_currentLevelNumber, typeToLoad);
                InitializeLevelBuffer(_currentLevelNumber, _currentLevel);

                labLevelInfo.Text = $"Level {_currentLevelNumber} ({_currentMode}): {_currentLevel.AuthorOrDescription}";
                UpdateTargetDisplayAndHighlight();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed: {ex.Message}. CONTACT DEVELOPER.");
            }
            finally
            {
                hotkeyLevelReroll = false;
            }
        }

        private void UpdateStats()
        {
            double minutes = _secondsElapsed / 60.0;
            int charsTypedCurrent = startPromptIsShown ? 0 : rtbInput.Text.Length;
            int totalCharsSoFar = _totalCharsTyped + charsTypedCurrent;
            labAccuracy.Text = $"Accuracy: {GetAccuracyPercent():0.0}%";
            labLevel.Text = $"Level {_currentLevelNumber}";
        }

        private int GetCurrentWpm()
        {
            double minutes = _secondsElapsed / 60.0;
            if (minutes <= 0) return 0;
            int chars = (startPromptIsShown ? 0 : rtbInput.Text.Length) + _totalCharsTyped;
            return (int)((chars / 5.0) / minutes);
        }

        private double GetAccuracyPercent()
        {
            int chars = (startPromptIsShown ? 0 : rtbInput.Text.Length) + _totalCharsTyped;
            if (chars <= 0) return 100.0;
            double correct = Math.Max(0, chars - _totalSessionErrors);
            return (correct / chars) * 100.0;
        }

        private void SaveScoreSnapshot()
        {
            try
            {
                var entry = new ScoreEntry
                {
                    Mode = _currentMode,
                    LevelReached = _currentLevelNumber,
                    TimeSeconds = _secondsElapsed,
                    Wpm = GetCurrentWpm(),
                    Accuracy = GetAccuracyPercent(),
                    Errors = _totalSessionErrors,
                    Timestamp = DateTime.Now
                };
                scoreStorage.SaveScore(entry);
            }
            catch { }
        }

        private Color GetTargetDefaultTextColor() => _isDarkMode ? Color.LightGray : Color.Black;

        private void LevelComplete()
        {
            int currentWPM = 0;
            int.TryParse(labWPM.Text.Replace("WPM: ", ""), out currentWPM);
            labLastScore.Text = $"Lvl {_currentLevelNumber} Done! ({currentWPM} WPM)";
            labLastScore.ForeColor = Color.LimeGreen;

            _currentLevelNumber++;
            _lastInputLength = 0;
            StartGame();
        }


        private void btnScores_Click(object sender, EventArgs e)
        {
            using (var scores = new ScoreForm(_isDarkMode))
            {
                scores.ShowDialog(this);
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent) => SendMessage(parent.Handle, WM_SETREDRAW, false, 0);

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        private void SetInputPlaceholder()
        {
            if (_isGameRunning) return;
            if (rtbInput.Focused) return;

            startPromptIsShown = true;
            rtbInput.Text = startPromptText;
            rtbInput.SelectionStart = 0;
            rtbInput.SelectionLength = 0;
            rtbInput.ReadOnly = true;
            rtbInput.ForeColor = ThemeColor.PlaceholderText;
        }

        private void RemoveInputPlaceholder()
        {
            if (!startPromptIsShown) return;

            startPromptIsShown = false;
            rtbInput.Text = "";
            rtbInput.ReadOnly = false;
            ApplyTheme(); 
        }
        private void WireUpNonDesignerEvents()
        {
            rtbInput.Enter += (_, __) => RemoveInputPlaceholder();
            rtbInput.Leave += (_, __) => SetInputPlaceholder();
            rtbInput.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {

                    e.SuppressKeyPress = true;
                    TryAdvanceLine(forceCorrect: false);
                    return;
                }

                if (startPromptIsShown)
                {
                    RemoveInputPlaceholder();
                }
                if (_isGameRunning && !gameTimer.Enabled)
                {
                    gameTimer.Start();
                }
            };


            this.KeyDown += MainForm_KeyDown;
        }
    }
}
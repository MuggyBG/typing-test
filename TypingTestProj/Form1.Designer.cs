using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TypingTest_Project
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            labHint = new Label();
            btnThemeToggle = new Button();
            labTitle = new Label();
            pnlInfo = new Panel();
            labLevelInfo = new Label();
            pnlGame = new Panel();
            pnlStats = new Panel();
            btnFinish = new Button();
            btnScores = new Button();
            label1 = new Label();
            labLastScore = new Label();
            labLevel = new Label();
            labTimer = new Label();
            labAccuracy = new Label();
            labWPM = new Label();
            rtbInput = new RichTextBox();
            rtbTarget = new RichTextBox();
            pnlMenu = new Panel();
            btnScoreboard2 = new Button();
            btnModeEndless = new Button();
            btnModeExtended = new Button();
            btnModeCode = new Button();
            btnModeStandard = new Button();
            gameTimer = new System.Windows.Forms.Timer(components);
            pnlHeader.SuspendLayout();
            pnlInfo.SuspendLayout();
            pnlGame.SuspendLayout();
            pnlStats.SuspendLayout();
            pnlMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(45, 45, 48);
            pnlHeader.Controls.Add(labHint);
            pnlHeader.Controls.Add(btnThemeToggle);
            pnlHeader.Controls.Add(labTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1125, 60);
            pnlHeader.TabIndex = 0;
            // 
            // labHint
            // 
            labHint.Dock = DockStyle.Left;
            labHint.ForeColor = SystemColors.Control;
            labHint.Location = new Point(0, 0);
            labHint.Name = "labHint";
            labHint.Size = new Size(140, 60);
            labHint.TabIndex = 2;
            labHint.Text = "F1 - Reroll line      F3 - Complete level Esc - End Game";
            // 
            // btnThemeToggle
            // 
            btnThemeToggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThemeToggle.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemeToggle.ForeColor = Color.Black;
            btnThemeToggle.Location = new Point(1031, 9);
            btnThemeToggle.Name = "btnThemeToggle";
            btnThemeToggle.Size = new Size(71, 42);
            btnThemeToggle.TabIndex = 1;
            btnThemeToggle.Text = "☀/☾";
            btnThemeToggle.UseVisualStyleBackColor = true;
            btnThemeToggle.Click += btnThemeToggle_Click;
            // 
            // labTitle
            // 
            labTitle.Anchor = AnchorStyles.Top;
            labTitle.AutoSize = true;
            labTitle.Font = new System.Drawing.Font("Comic Sans MS", 16F, FontStyle.Bold);
            labTitle.ForeColor = Color.White;
            labTitle.Location = new Point(487, 15);
            labTitle.Name = "labTitle";
            labTitle.Size = new Size(141, 31);
            labTitle.TabIndex = 0;
            labTitle.Text = "Typing Test";
            // 
            // pnlInfo
            // 
            pnlInfo.BackColor = Color.FromArgb(0, 122, 204);
            pnlInfo.Controls.Add(labLevelInfo);
            pnlInfo.Dock = DockStyle.Top;
            pnlInfo.Location = new Point(0, 60);
            pnlInfo.Name = "pnlInfo";
            pnlInfo.Size = new Size(1125, 100);
            pnlInfo.TabIndex = 1;
            // 
            // labLevelInfo
            // 
            labLevelInfo.AutoSize = true;
            labLevelInfo.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            labLevelInfo.ForeColor = Color.White;
            labLevelInfo.Location = new Point(20, 15);
            labLevelInfo.Name = "labLevelInfo";
            labLevelInfo.Size = new Size(164, 27);
            labLevelInfo.TabIndex = 0;
            labLevelInfo.Text = "Level 1: Loading...";
            labLevelInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlGame
            // 
            pnlGame.Controls.Add(pnlStats);
            pnlGame.Controls.Add(rtbInput);
            pnlGame.Controls.Add(rtbTarget);
            pnlGame.Controls.Add(pnlMenu);
            pnlGame.Dock = DockStyle.Fill;
            pnlGame.Location = new Point(0, 160);
            pnlGame.Name = "pnlGame";
            pnlGame.Padding = new Padding(50, 20, 50, 20);
            pnlGame.Size = new Size(1125, 562);
            pnlGame.TabIndex = 1;
            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.FromArgb(40, 40, 40);
            pnlStats.Controls.Add(btnFinish);
            pnlStats.Controls.Add(btnScores);
            pnlStats.Controls.Add(label1);
            pnlStats.Controls.Add(labLastScore);
            pnlStats.Controls.Add(labLevel);
            pnlStats.Controls.Add(labTimer);
            pnlStats.Controls.Add(labAccuracy);
            pnlStats.Controls.Add(labWPM);
            pnlStats.Dock = DockStyle.Bottom;
            pnlStats.Location = new Point(50, 442);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new Size(1025, 100);
            pnlStats.TabIndex = 2;
            // 
            // btnFinish
            // 
            btnFinish.Cursor = Cursors.Hand;
            btnFinish.Location = new Point(150, 3);
            btnFinish.Name = "btnFinish";
            btnFinish.Size = new Size(80, 30);
            btnFinish.TabIndex = 5;
            btnFinish.Text = "Finish";
            btnFinish.UseVisualStyleBackColor = true;
            btnFinish.Click += btnFinish_Click;
            // 
            // btnScores
            // 
            btnScores.Cursor = Cursors.Hand;
            btnScores.Location = new Point(64, 3);
            btnScores.Name = "btnScores";
            btnScores.Size = new Size(80, 30);
            btnScores.TabIndex = 6;
            btnScores.Text = "Scores";
            btnScores.UseVisualStyleBackColor = true;
            btnScores.Click += btnScores_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Comic Sans MS", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LightGray;
            label1.Location = new Point(585, 9);
            label1.Name = "label1";
            label1.Size = new Size(99, 35);
            label1.TabIndex = 4;
            label1.Text = "Last: -";
            // 
            // labLastScore
            // 
            labLastScore.AutoSize = true;
            labLastScore.Font = new System.Drawing.Font("Comic Sans MS", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labLastScore.ForeColor = Color.LightGray;
            labLastScore.Location = new Point(609, 9);
            labLastScore.Name = "labLastScore";
            labLastScore.Size = new Size(99, 35);
            labLastScore.TabIndex = 4;
            labLastScore.Text = "Last: -";
            // 
            // labLevel
            // 
            labLevel.AutoSize = true;
            labLevel.Font = new System.Drawing.Font("Comic Sans MS", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labLevel.ForeColor = Color.White;
            labLevel.Location = new Point(437, 9);
            labLevel.Name = "labLevel";
            labLevel.Size = new Size(98, 35);
            labLevel.TabIndex = 3;
            labLevel.Text = "Level 0";
            // 
            // labTimer
            // 
            labTimer.AutoSize = true;
            labTimer.Font = new System.Drawing.Font("Comic Sans MS", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labTimer.ForeColor = Color.White;
            labTimer.Location = new Point(766, 50);
            labTimer.Name = "labTimer";
            labTimer.Size = new Size(215, 35);
            labTimer.TabIndex = 2;
            labTimer.Text = "Time Elapsed: 0s";
            // 
            // labAccuracy
            // 
            labAccuracy.AutoSize = true;
            labAccuracy.Font = new System.Drawing.Font("Comic Sans MS", 18F);
            labAccuracy.ForeColor = Color.Gold;
            labAccuracy.Location = new Point(398, 52);
            labAccuracy.Name = "labAccuracy";
            labAccuracy.Size = new Size(191, 33);
            labAccuracy.TabIndex = 1;
            labAccuracy.Text = "Accuracy: 100%";
            // 
            // labWPM
            // 
            labWPM.AutoSize = true;
            labWPM.Font = new System.Drawing.Font("Comic Sans MS", 18F);
            labWPM.ForeColor = Color.LimeGreen;
            labWPM.Location = new Point(64, 50);
            labWPM.Name = "labWPM";
            labWPM.Size = new Size(103, 33);
            labWPM.TabIndex = 0;
            labWPM.Text = "WPM: 0";
            // 
            // rtbInput
            // 
            rtbInput.BackColor = Color.FromArgb(30, 30, 30);
            rtbInput.Dock = DockStyle.Top;
            rtbInput.Font = new System.Drawing.Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbInput.ForeColor = Color.White;
            rtbInput.Location = new Point(50, 162);
            rtbInput.Name = "rtbInput";
            rtbInput.ReadOnly = true;
            rtbInput.Size = new Size(1025, 176);
            rtbInput.TabIndex = 1;
            rtbInput.Text = "Enter text here...";
            rtbInput.TextChanged += rtbInput_TextChanged;
            // 
            // rtbTarget
            // 
            rtbTarget.BackColor = Color.FromArgb(30, 30, 30);
            rtbTarget.BorderStyle = BorderStyle.None;
            rtbTarget.Dock = DockStyle.Top;
            rtbTarget.Font = new System.Drawing.Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbTarget.ForeColor = Color.FromArgb(150, 150, 150);
            rtbTarget.Location = new Point(50, 20);
            rtbTarget.Name = "rtbTarget";
            rtbTarget.ReadOnly = true;
            rtbTarget.Size = new Size(1025, 142);
            rtbTarget.TabIndex = 0;
            rtbTarget.Text = "Press SPACE or the Start button to start...";
            // 
            // pnlMenu
            // 
            pnlMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlMenu.Controls.Add(btnScoreboard2);
            pnlMenu.Controls.Add(btnModeEndless);
            pnlMenu.Controls.Add(btnModeExtended);
            pnlMenu.Controls.Add(btnModeCode);
            pnlMenu.Controls.Add(btnModeStandard);
            pnlMenu.Location = new Point(50, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(1025, 442);
            pnlMenu.TabIndex = 1;
            // 
            // btnScoreboard2
            // 
            btnScoreboard2.Cursor = Cursors.Hand;
            btnScoreboard2.Location = new Point(799, 394);
            btnScoreboard2.Name = "btnScoreboard2";
            btnScoreboard2.Size = new Size(226, 48);
            btnScoreboard2.TabIndex = 10;
            btnScoreboard2.Text = "Scoreboard";
            btnScoreboard2.UseVisualStyleBackColor = true;
            btnScoreboard2.Click += btnScores_Click;
            // 
            // btnModeEndless
            // 
            btnModeEndless.Cursor = Cursors.Hand;
            btnModeEndless.Font = new System.Drawing.Font("Comic Sans MS", 20F);
            btnModeEndless.Location = new Point(773, 93);
            btnModeEndless.Name = "btnModeEndless";
            btnModeEndless.Size = new Size(208, 215);
            btnModeEndless.TabIndex = 9;
            btnModeEndless.Text = "Endless Mode";
            btnModeEndless.UseVisualStyleBackColor = true;
            btnModeEndless.Click += btnModeEndless_Click;
            // 
            // btnModeExtended
            // 
            btnModeExtended.Cursor = Cursors.Hand;
            btnModeExtended.Font = new System.Drawing.Font("Comic Sans MS", 20F);
            btnModeExtended.Location = new Point(282, 93);
            btnModeExtended.Name = "btnModeExtended";
            btnModeExtended.Size = new Size(208, 213);
            btnModeExtended.TabIndex = 8;
            btnModeExtended.Text = "Hard Mode: 100 Levels";
            btnModeExtended.UseVisualStyleBackColor = true;
            btnModeExtended.Click += btnModeExtended_Click;
            // 
            // btnModeCode
            // 
            btnModeCode.Cursor = Cursors.Hand;
            btnModeCode.Font = new System.Drawing.Font("Comic Sans MS", 20F);
            btnModeCode.Location = new Point(531, 93);
            btnModeCode.Name = "btnModeCode";
            btnModeCode.Size = new Size(208, 213);
            btnModeCode.TabIndex = 7;
            btnModeCode.Text = "Code Mode: ONLY CODE";
            btnModeCode.UseVisualStyleBackColor = true;
            btnModeCode.Click += btnModeCode_Click;
            // 
            // btnModeStandard
            // 
            btnModeStandard.Cursor = Cursors.Hand;
            btnModeStandard.Font = new System.Drawing.Font("Comic Sans MS", 20F);
            btnModeStandard.Location = new Point(36, 93);
            btnModeStandard.Name = "btnModeStandard";
            btnModeStandard.Size = new Size(208, 213);
            btnModeStandard.TabIndex = 6;
            btnModeStandard.Text = "Normal Mode: 50 Levels";
            btnModeStandard.UseVisualStyleBackColor = true;
            btnModeStandard.Click += btnModeStandard_Click;
            // 
            // gameTimer
            // 
            gameTimer.Interval = 1000;
            gameTimer.Tick += gameTimer_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1125, 722);
            Controls.Add(pnlGame);
            Controls.Add(pnlInfo);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Comic Sans MS", 10F);
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Typing Game";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlInfo.ResumeLayout(false);
            pnlInfo.PerformLayout();
            pnlGame.ResumeLayout(false);
            pnlStats.ResumeLayout(false);
            pnlStats.PerformLayout();
            pnlMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label labTitle;
        private Button btnThemeToggle;
        private Panel pnlInfo;
        private Label labLevelInfo;
        private Panel pnlGame;
        private RichTextBox rtbTarget;
        private RichTextBox rtbInput;
        private Panel pnlStats;
        private Label labWPM;
        private Label labTimer;
        private Label labAccuracy;
        private System.Windows.Forms.Timer gameTimer;
        private Label labLevel;
        private Label labLastScore;
        private Panel pnlMenu;
        private Button btnFinish;
        private Button btnScores;
        private Button btnModeEndless;
        private Button btnModeExtended;
        private Button btnModeCode;
        private Button btnModeStandard;
        private Label labHint;
        private Button btnScoreboard2;
        private Label label1;
    }
}
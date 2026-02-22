namespace TypingTest_Project
{
    partial class ScoreForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView lvwScores;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnClose;
        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lvwScores = new System.Windows.Forms.ListView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // Header Panel
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 64;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            // Title Label
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Text = "Scoreboard (sorted by WPM)";

            // ListView
            this.lvwScores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwScores.View = System.Windows.Forms.View.Details;
            this.lvwScores.FullRowSelect = true;
            this.lvwScores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwScores.Font = new System.Drawing.Font("Consolas", 11F);

            this.lvwScores.Columns.Add("WPM", 80, System.Windows.Forms.HorizontalAlignment.Right);
            this.lvwScores.Columns.Add("Accuracy", 100, System.Windows.Forms.HorizontalAlignment.Right);
            this.lvwScores.Columns.Add("Errors", 80, System.Windows.Forms.HorizontalAlignment.Right);
            this.lvwScores.Columns.Add("Level", 70, System.Windows.Forms.HorizontalAlignment.Right);
            this.lvwScores.Columns.Add("Mode", 120, System.Windows.Forms.HorizontalAlignment.Left);
            this.lvwScores.Columns.Add("Time (s)", 90, System.Windows.Forms.HorizontalAlignment.Right);
            this.lvwScores.Columns.Add("When", 170, System.Windows.Forms.HorizontalAlignment.Left);

            // Footer Panel
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 70;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);

            // Close Button
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Width = 120;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Text = "Close";

            // Assembly
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlFooter.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvwScores);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);

            // Form Settings
            this.ClientSize = new System.Drawing.Size(760, 540);
            this.Text = "Scoreboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        
    }

        #endregion
    }
}
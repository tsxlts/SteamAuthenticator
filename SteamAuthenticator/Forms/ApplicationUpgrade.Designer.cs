using Steam_Authenticator.Controls;

namespace Steam_Authenticator.Forms
{
    partial class ApplicationUpgrade
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationUpgrade));
            label1 = new Label();
            currentVersionBox = new Label();
            latestVersionBox = new Label();
            label3 = new Label();
            panel1 = new Panel();
            versionSummaryBox = new ReadonlyRichTextBox();
            label2 = new Label();
            latestVersionTimeBox = new Label();
            label5 = new Label();
            panel2 = new Panel();
            progress = new Label();
            progressBar = new ProgressBar();
            downloadBtn = new LinkLabel();
            upgradeBtn = new LinkLabel();
            giteeDownload = new Label();
            label6 = new Label();
            githubDownload = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(73, 23);
            label1.TabIndex = 0;
            label1.Text = "当前版本：";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // currentVersionBox
            // 
            currentVersionBox.ImageAlign = ContentAlignment.MiddleLeft;
            currentVersionBox.Location = new Point(91, 9);
            currentVersionBox.Name = "currentVersionBox";
            currentVersionBox.Size = new Size(73, 23);
            currentVersionBox.TabIndex = 1;
            currentVersionBox.Text = "0.0.0";
            currentVersionBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // latestVersionBox
            // 
            latestVersionBox.ForeColor = Color.Green;
            latestVersionBox.ImageAlign = ContentAlignment.MiddleLeft;
            latestVersionBox.Location = new Point(91, 32);
            latestVersionBox.Name = "latestVersionBox";
            latestVersionBox.Size = new Size(73, 23);
            latestVersionBox.TabIndex = 3;
            latestVersionBox.Text = "0.0.0";
            latestVersionBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.ForeColor = Color.Green;
            label3.ImageAlign = ContentAlignment.MiddleLeft;
            label3.Location = new Point(12, 32);
            label3.Name = "label3";
            label3.Size = new Size(73, 23);
            label3.TabIndex = 2;
            label3.Text = "最新版本：";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(versionSummaryBox);
            panel1.Location = new Point(12, 81);
            panel1.Name = "panel1";
            panel1.Size = new Size(685, 320);
            panel1.TabIndex = 4;
            // 
            // versionSummaryBox
            // 
            versionSummaryBox.BorderStyle = BorderStyle.None;
            versionSummaryBox.Dock = DockStyle.Fill;
            versionSummaryBox.Location = new Point(0, 0);
            versionSummaryBox.Name = "versionSummaryBox";
            versionSummaryBox.ReadOnly = true;
            versionSummaryBox.Size = new Size(685, 320);
            versionSummaryBox.TabIndex = 0;
            versionSummaryBox.Text = "";
            // 
            // label2
            // 
            label2.ImageAlign = ContentAlignment.MiddleLeft;
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(73, 23);
            label2.TabIndex = 5;
            label2.Text = "更新内容：";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // latestVersionTimeBox
            // 
            latestVersionTimeBox.ForeColor = Color.Green;
            latestVersionTimeBox.ImageAlign = ContentAlignment.MiddleLeft;
            latestVersionTimeBox.Location = new Point(249, 32);
            latestVersionTimeBox.Name = "latestVersionTimeBox";
            latestVersionTimeBox.Size = new Size(213, 23);
            latestVersionTimeBox.TabIndex = 7;
            latestVersionTimeBox.Text = "0.0.0";
            latestVersionTimeBox.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.ForeColor = Color.Green;
            label5.ImageAlign = ContentAlignment.MiddleLeft;
            label5.Location = new Point(170, 32);
            label5.Name = "label5";
            label5.Size = new Size(73, 23);
            label5.TabIndex = 6;
            label5.Text = "发布时间：";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(progress);
            panel2.Controls.Add(progressBar);
            panel2.Controls.Add(downloadBtn);
            panel2.Controls.Add(upgradeBtn);
            panel2.Location = new Point(12, 407);
            panel2.Name = "panel2";
            panel2.Size = new Size(685, 36);
            panel2.TabIndex = 8;
            // 
            // progress
            // 
            progress.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            progress.ForeColor = Color.Green;
            progress.Location = new Point(463, 10);
            progress.Name = "progress";
            progress.Size = new Size(63, 17);
            progress.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(5, 7);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(452, 23);
            progressBar.TabIndex = 2;
            // 
            // downloadBtn
            // 
            downloadBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            downloadBtn.AutoSize = true;
            downloadBtn.Cursor = Cursors.Hand;
            downloadBtn.Location = new Point(611, 10);
            downloadBtn.Name = "downloadBtn";
            downloadBtn.Size = new Size(68, 17);
            downloadBtn.TabIndex = 1;
            downloadBtn.TabStop = true;
            downloadBtn.Text = "无法更新？";
            downloadBtn.LinkClicked += downloadBtn_LinkClicked;
            // 
            // upgradeBtn
            // 
            upgradeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            upgradeBtn.AutoSize = true;
            upgradeBtn.Cursor = Cursors.Hand;
            upgradeBtn.Location = new Point(542, 10);
            upgradeBtn.Name = "upgradeBtn";
            upgradeBtn.Size = new Size(56, 17);
            upgradeBtn.TabIndex = 0;
            upgradeBtn.TabStop = true;
            upgradeBtn.Text = "立即更新";
            upgradeBtn.LinkClicked += upgradeBtn_LinkClicked;
            // 
            // giteeDownload
            // 
            giteeDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            giteeDownload.Cursor = Cursors.Hand;
            giteeDownload.ForeColor = Color.Blue;
            giteeDownload.ImageAlign = ContentAlignment.MiddleLeft;
            giteeDownload.Location = new Point(575, 32);
            giteeDownload.Name = "giteeDownload";
            giteeDownload.Size = new Size(57, 23);
            giteeDownload.TabIndex = 10;
            giteeDownload.Text = "国内链接";
            giteeDownload.TextAlign = ContentAlignment.MiddleLeft;
            giteeDownload.Click += giteeDownload_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.ForeColor = Color.Gray;
            label6.ImageAlign = ContentAlignment.MiddleLeft;
            label6.Location = new Point(475, 32);
            label6.Name = "label6";
            label6.Size = new Size(94, 23);
            label6.TabIndex = 9;
            label6.Text = "复制下载链接：";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // githubDownload
            // 
            githubDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            githubDownload.Cursor = Cursors.Hand;
            githubDownload.ForeColor = Color.Blue;
            githubDownload.ImageAlign = ContentAlignment.MiddleLeft;
            githubDownload.Location = new Point(638, 32);
            githubDownload.Name = "githubDownload";
            githubDownload.Size = new Size(57, 23);
            githubDownload.TabIndex = 11;
            githubDownload.Text = "国际链接";
            githubDownload.TextAlign = ContentAlignment.MiddleLeft;
            githubDownload.Click += githubDownload_Click;
            // 
            // ApplicationUpgrade
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(709, 451);
            Controls.Add(githubDownload);
            Controls.Add(giteeDownload);
            Controls.Add(label6);
            Controls.Add(panel2);
            Controls.Add(latestVersionTimeBox);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(latestVersionBox);
            Controls.Add(label3);
            Controls.Add(currentVersionBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ApplicationUpgrade";
            StartPosition = FormStartPosition.CenterParent;
            Text = "检查更新";
            FormClosing += ApplicationUpgrade_FormClosing;
            Load += ApplicationUpgrade_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label currentVersionBox;
        private Label latestVersionBox;
        private Label label3;
        private Panel panel1;
        private Label label2;
        private Label latestVersionTimeBox;
        private Label label5;
        private ReadonlyRichTextBox versionSummaryBox;
        private Panel panel2;
        private LinkLabel upgradeBtn;
        private LinkLabel downloadBtn;
        private ProgressBar progressBar;
        private Label progress;
        private Label giteeDownload;
        private Label label6;
        private Label githubDownload;
    }
}
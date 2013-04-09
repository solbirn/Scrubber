namespace Scrubber
{
    partial class ScrubberGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScrubberGUI));
            this.DebugPrintConsole = new System.Windows.Forms.TextBox();
            this.Settings = new System.Windows.Forms.Button();
            this.scrub = new System.Windows.Forms.Button();
            this.CloseC = new System.Windows.Forms.Button();
            this.ClearLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DebugPrintConsole
            // 
            this.DebugPrintConsole.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebugPrintConsole.Location = new System.Drawing.Point(12, 12);
            this.DebugPrintConsole.Multiline = true;
            this.DebugPrintConsole.Name = "DebugPrintConsole";
            this.DebugPrintConsole.ReadOnly = true;
            this.DebugPrintConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DebugPrintConsole.Size = new System.Drawing.Size(1085, 577);
            this.DebugPrintConsole.TabIndex = 0;
            // 
            // Settings
            // 
            this.Settings.Location = new System.Drawing.Point(941, 594);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(75, 23);
            this.Settings.TabIndex = 1;
            this.Settings.Text = "Settings...";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // scrub
            // 
            this.scrub.Location = new System.Drawing.Point(860, 594);
            this.scrub.Name = "scrub";
            this.scrub.Size = new System.Drawing.Size(75, 23);
            this.scrub.TabIndex = 2;
            this.scrub.Text = "Scrub Again";
            this.scrub.UseVisualStyleBackColor = true;
            this.scrub.Click += new System.EventHandler(this.scrub_Click);
            // 
            // CloseC
            // 
            this.CloseC.Location = new System.Drawing.Point(1022, 594);
            this.CloseC.Name = "CloseC";
            this.CloseC.Size = new System.Drawing.Size(75, 23);
            this.CloseC.TabIndex = 3;
            this.CloseC.Text = "Exit";
            this.CloseC.UseVisualStyleBackColor = true;
            this.CloseC.Click += new System.EventHandler(this.CloseC_Click);
            // 
            // ClearLog
            // 
            this.ClearLog.Location = new System.Drawing.Point(13, 595);
            this.ClearLog.Name = "ClearLog";
            this.ClearLog.Size = new System.Drawing.Size(75, 23);
            this.ClearLog.TabIndex = 4;
            this.ClearLog.Text = "Clear";
            this.ClearLog.UseVisualStyleBackColor = true;
            this.ClearLog.Click += new System.EventHandler(this.ClearLog_Click);
            // 
            // ScrubberGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 624);
            this.Controls.Add(this.ClearLog);
            this.Controls.Add(this.CloseC);
            this.Controls.Add(this.scrub);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.DebugPrintConsole);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScrubberGUI";
            this.Text = "Scrubber";
            this.Shown += new System.EventHandler(this.ScrubberGUI_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DebugPrintConsole;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Button scrub;
        private System.Windows.Forms.Button CloseC;
        private System.Windows.Forms.Button ClearLog;
    }
}
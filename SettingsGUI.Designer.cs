namespace Scrubber
{
    partial class SettingsGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsGUI));
            this.Keywords = new System.Windows.Forms.ListBox();
            this.Encrypt = new System.Windows.Forms.CheckBox();
            this.NewKeyword = new System.Windows.Forms.TextBox();
            this.Add = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.CloseC = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.closecomplete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Keywords
            // 
            this.Keywords.AccessibleName = "Keywords";
            this.Keywords.FormattingEnabled = true;
            this.Keywords.Location = new System.Drawing.Point(12, 44);
            this.Keywords.Name = "Keywords";
            this.Keywords.Size = new System.Drawing.Size(272, 212);
            this.Keywords.TabIndex = 6;
            // 
            // Encrypt
            // 
            this.Encrypt.AutoSize = true;
            this.Encrypt.Enabled = false;
            this.Encrypt.Location = new System.Drawing.Point(12, 294);
            this.Encrypt.Name = "Encrypt";
            this.Encrypt.Size = new System.Drawing.Size(110, 17);
            this.Encrypt.TabIndex = 2;
            this.Encrypt.Text = "Encrypt keywords";
            this.Encrypt.UseVisualStyleBackColor = true;
            this.Encrypt.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // NewKeyword
            // 
            this.NewKeyword.Location = new System.Drawing.Point(12, 18);
            this.NewKeyword.Name = "NewKeyword";
            this.NewKeyword.Size = new System.Drawing.Size(204, 20);
            this.NewKeyword.TabIndex = 0;
            // 
            // Add
            // 
            this.Add.AccessibleName = "Add";
            this.Add.Location = new System.Drawing.Point(227, 18);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(57, 20);
            this.Add.TabIndex = 1;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.button1_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(128, 291);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 3;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // CloseC
            // 
            this.CloseC.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseC.Location = new System.Drawing.Point(209, 291);
            this.CloseC.Name = "CloseC";
            this.CloseC.Size = new System.Drawing.Size(75, 23);
            this.CloseC.TabIndex = 4;
            this.CloseC.Text = "Close";
            this.CloseC.UseVisualStyleBackColor = true;
            this.CloseC.Click += new System.EventHandler(this.Abort_Click);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(209, 262);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 5;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.button4_Click);
            // 
            // closecomplete
            // 
            this.closecomplete.AutoSize = true;
            this.closecomplete.Location = new System.Drawing.Point(12, 268);
            this.closecomplete.Name = "closecomplete";
            this.closecomplete.Size = new System.Drawing.Size(175, 17);
            this.closecomplete.TabIndex = 7;
            this.closecomplete.Text = "Autoclose Scrubber when done";
            this.closecomplete.UseVisualStyleBackColor = true;
            // 
            // SettingsGUI
            // 
            this.AcceptButton = this.Add;
            this.AccessibleName = "Scrubber Settings";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseC;
            this.ClientSize = new System.Drawing.Size(297, 323);
            this.Controls.Add(this.closecomplete);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.CloseC);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.NewKeyword);
            this.Controls.Add(this.Encrypt);
            this.Controls.Add(this.Keywords);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsGUI";
            this.Text = "Scrubber Settings";
            this.Load += new System.EventHandler(this.SettingsGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Keywords;
        private System.Windows.Forms.CheckBox Encrypt;
        private System.Windows.Forms.TextBox NewKeyword;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button CloseC;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.CheckBox closecomplete;
    }
}
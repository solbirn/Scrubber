using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

using Scrubber.Extensions;

namespace Scrubber
{
    public partial class ScrubberGUI : Form
    {
        List<string> keywords;
        Scrubber.SettingsGUI settings;

        public ScrubberGUI()
        {
            InitializeComponent();
        }

        public ScrubberGUI(List<string> keywords)
        {
            this.keywords = keywords;
            settings = new Scrubber.SettingsGUI();

            InitializeComponent();
        }

        private void Scrub() 
        {
            DebugPrint("Scrubber Starting...");

            MRUCleaner mruc = new MRUCleaner(this.keywords, this);

            string[] JLFiles = Directory.GetFiles(Environment.ExpandEnvironmentVariables("%appdata%") + "\\Microsoft\\Windows\\Recent\\AutomaticDestinations");

            foreach (string JLFile in JLFiles)
            {
                JumpListCleaner cjl = new JumpListCleaner(JLFile, this.keywords, this);
            }

            DebugPrint("Scrubbing Complete.");

            if (this.settings.getAutoCloseSetting())
            {
                Application.Exit();
            }
        }

        private void ScrubT()
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(Scrub));
            t.Start();
        }

        private void ScrubberGUI_Shown(object sender, EventArgs e)
        {
            this.ScrubT();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(OpenSettings));
            t.Start();
        }

        public static void OpenSettings()
        {
            Application.Run(new SettingsGUI());
        }

        private void scrub_Click(object sender, EventArgs e)
        {
            this.ScrubT();
        }

        public void DebugPrint(string text)
        {
            this.DebugPrintConsole.SetPropertyThreadSafe(() => this.DebugPrintConsole.Text, this.DebugPrintConsole.Text + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " - " + text + "\r\n");
        }

        private void CloseC_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            this.DebugPrintConsole.Text = "";
        }
    }
}

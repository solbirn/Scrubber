using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scrubber
{
    public partial class SettingsGUI : Form
    {
        Properties.Settings settings = new Properties.Settings();
        string[] keywordssa;

        public SettingsGUI()
        {
            InitializeComponent();
        }

        public string[] getKeywordsFromRaw()
        {
            string rawkeys;
            string[] keywords;
            if (!this.settings.Encrypt)
            {
                rawkeys = this.settings.Keywords;
            }
            else
            {
                rawkeys = this.settings.Keywords;
            }
            keywords = rawkeys.Split('|');
            return keywords;
        }

        public bool getAutoCloseSetting()
        {
            return this.settings.Autoclose;
        }
        private void SettingsGUI_Load(object sender, EventArgs e)
        {
            this.keywordssa = getKeywordsFromRaw();
            foreach (string kw in this.keywordssa) 
            {
                if(!kw.Equals(""))
                this.Keywords.Items.Add(kw);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Keywords.Items.Add(NewKeyword.Text);
            NewKeyword.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Keywords.Items.RemoveAt(this.Keywords.SelectedIndex);
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string kws = "";
            foreach (object item in this.Keywords.Items)
            {
                if (!kws.Equals("")) kws += "|";
                kws += item.ToString();
            }
            this.settings.Keywords = kws;
            this.settings.Autoclose = this.closecomplete.Checked;
            this.settings.Save();
            this.Close();
        }
    }
}

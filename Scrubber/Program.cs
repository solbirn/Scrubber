namespace Scrubber
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Scrubber;

    internal static class Program
    {

        [STAThread]
        private static void Main(string[] args)
        {
            using (ShellLink shortcut = new ShellLink())
            {
                shortcut.Target = Application.ExecutablePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                shortcut.Arguments = "-s";
                shortcut.Description = "Settings GUI for Scrubber";
                shortcut.DisplayMode = ShellLink.LinkDisplayMode.edmNormal;
                shortcut.Save(Environment.ExpandEnvironmentVariables("%appdata%") + "\\Microsoft\\Windows\\Start Menu\\Programs\\Scrubber\\Scrubber Settings.lnk");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Scrubber.SettingsGUI settings = new Scrubber.SettingsGUI();

            List<string> keywords = new List<string>(args);

            if (keywords.Count > 0)
            {
                if (keywords[0].StartsWith("-") || keywords[0].StartsWith("/"))
                {
                    if (keywords[0].Equals("-s") || keywords[0].Equals("--settings") || keywords[0].Equals("/s"))
                    {
                        Application.Run(settings);
                        Application.Exit();
                        return;
                    }
                    keywords.RemoveAt(0);
                }
            }
            

            foreach (string kw in settings.getKeywordsFromRaw())
            {
                if (!kw.Equals(""))
                    keywords.Add(kw);
            }



            if (keywords.Count != 0)
            {
                ScrubberGUI GUI = new ScrubberGUI(keywords);
                Application.Run(GUI);
            }
            else
            {
                Application.Run(settings);
                Thread t = new Thread(() => OpenApp(keywords));
                t.Start();
            }

            
        }
        public static void OpenApp(List<string> kw)
        {
            Application.Run(new ScrubberGUI(kw));
        }
        
    }
}


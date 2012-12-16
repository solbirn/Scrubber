namespace Scrubber
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class JumpListFile
    {
        public JumpListFile()
        {
            this.FilePath = string.Empty;
            this.FileName = string.Empty;
            this.JumpLists = new List<JumpList>();
            this.DestListEntries = new List<DestListEntry>();
        }

        public string AppName { get; set; }

        public List<DestListEntry> DestListEntries { get; set; }

        public long DestListSize { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public List<JumpList> JumpLists { get; set; }
    }
}


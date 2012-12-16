namespace Scrubber
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    public class JumpList
    {
        public JumpList()
        {
            this.Data = new KeyValuePair<string, string>();
            this.DestListEntry = new Scrubber.DestListEntry();
        }

        public KeyValuePair<string, string> Data { get; set; }

        public Scrubber.DestListEntry DestListEntry { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }
    }
}


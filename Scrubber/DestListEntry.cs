namespace Scrubber
{
    using System;
    using System.Runtime.CompilerServices;

    public class DestListEntry
    {
        public DestListEntry()
        {
            this.NetBiosName = string.Empty;
            this.StreamNo = string.Empty;
            this.StreamNoInt = 0x0;
            this.EntryPosition = 0x0;
            this.EntrySize = 0x0;
            this.Data = string.Empty;
            this.PendingDelete = false;
        }

        public string Data { get; set; }

        public byte[] RawData { get; set; }

        public DateTime FileTime { get; set; }

        public string NetBiosName { get; set; }

        public string StreamNo { get; set; }

        public long StreamNoInt { get; set; }

        public long EntryPosition { get; set; }

        public long EntrySize { get; set; }

        public bool PendingDelete { get; set; }
    }
}


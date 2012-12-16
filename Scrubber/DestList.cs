

namespace Scrubber
{
    using System;
    using System.Runtime.CompilerServices;

    class DestList
    {
        public DestList() {
            this.Header = new byte[0x20L];
        }

        public byte[] Header { get; set; }
    }
}

namespace Scrubber
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    using OpenMcdf;

    public class JumpListCleaner
    {
        private Dictionary<short, long>  positionsRelative;
        private Dictionary<short, long>  positionsAbsolute;
        private Dictionary<short, string> numPositionToName;
        ScrubberGUI ScrubberGUIInst;
        private List<JumpListFile> _jumpListFiles;
        private List<long> matches;
        private JumpListFile JumpListFiles;
        private DestList DestListObj;
        private List<string> KeywordsList;
        private string PathToJumpListFiles;

        public JumpListCleaner(string PathToJumpListFiles, List<string> KeywordsList, ScrubberGUI ScrubberGUIInst)
        {
            this.PathToJumpListFiles = PathToJumpListFiles;
            this.KeywordsList = KeywordsList;
            this.ScrubberGUIInst = ScrubberGUIInst;

            this.InitalizeVariables();

            this.ParseJumpList(this.PathToJumpListFiles);
            this.DeleteUnwantedJumpListEntries();
        }

        private void InitalizeVariables(){
            numPositionToName = new Dictionary<short, string>();
            numPositionToName.Add(1, "Header");
            numPositionToName.Add(2, "ToUnknownP"); //recurse starts here
            numPositionToName.Add(3, "ToPadding1");
            numPositionToName.Add(4, "NetBIOS");
            numPositionToName.Add(5, "StreamNo");
            numPositionToName.Add(6, "ToPadding2");
            numPositionToName.Add(7, "FileTime");
            numPositionToName.Add(8, "ToPadding3");
            numPositionToName.Add(9, "PathLength");
            numPositionToName.Add(10, "FilePath");

            positionsRelative = new Dictionary<short, long>();
            positionsRelative.Add(1, 0x00L);
            positionsRelative.Add(2, 0x20L); //recurse starts here
            positionsRelative.Add(3, 0x08L);
            positionsRelative.Add(4, 0x40L);
            positionsRelative.Add(5, 0x10L);
            positionsRelative.Add(6, 0x08L);
            positionsRelative.Add(7, 0x04L);
            positionsRelative.Add(8, 0x08L);
            positionsRelative.Add(9, 0x04L);
            positionsRelative.Add(10, 0x02L);

            positionsAbsolute = new Dictionary<short, long>();
            positionsAbsolute.Add(1, 0x00L);
            positionsAbsolute.Add(2, 0x20L); //recurse starts here
            positionsAbsolute.Add(3, 0x28L);
            positionsAbsolute.Add(4, 0x68L);
            positionsAbsolute.Add(5, 0x78L);
            positionsAbsolute.Add(6, 0x80L);
            positionsAbsolute.Add(7, 0x84L);
            positionsAbsolute.Add(8, 0x8CL);
            positionsAbsolute.Add(9, 0x90L);
            positionsAbsolute.Add(10, 0x92L);

            matches = new List<long>();

            _jumpListFiles = new List<JumpListFile>();

            DestListObj = new DestList();
        }


        private List<DestListEntry> ParseDestList(byte[] data)
        {
            
            List<DestListEntry> JumpListDestListEntries = new List<DestListEntry>();
            try
            {
                using (MemoryStream JumpListEntriesStream = new MemoryStream(data))
                {

                    JumpListEntriesStream.Read(this.DestListObj.Header, 0x0, 0x20);
                    do
                    {
                        DestListEntry JumpListDestListEntry = new DestListEntry();
                        JumpListDestListEntry.EntryPosition = JumpListEntriesStream.Position;
                        JumpListEntriesStream.Seek(0x48L, SeekOrigin.Current);
                        JumpListDestListEntry.NetBiosName = StreamHelper.ReadStr(JumpListEntriesStream, 0x10).Replace("\0", "");//Text.ReplaceNulls(StreamReaderHelper.ReadString(JumpListEntriesStream, 0x10));
                        JumpListDestListEntry.StreamNoInt = StreamHelper.Read64(JumpListEntriesStream);
                        JumpListDestListEntry.StreamNo = JumpListDestListEntry.StreamNoInt.ToString("X");
                        JumpListEntriesStream.Seek(4L, SeekOrigin.Current);
                        JumpListDestListEntry.FileTime = StreamHelper.ReadDateTime(JumpListEntriesStream);
                        JumpListEntriesStream.Seek(4L, SeekOrigin.Current);
                        int num = StreamHelper.Read16(JumpListEntriesStream);
                        if (num != -1)
                        {
                            JumpListDestListEntry.Data = StreamHelper.ReadStrU(JumpListEntriesStream, num * 2);
                        }
                        else
                        {
                            JumpListEntriesStream.Seek(4L, SeekOrigin.Current);
                        }
                        JumpListDestListEntry.Data = this.ReplaceNulls(JumpListDestListEntry.Data);
                        JumpListDestListEntry.EntrySize = JumpListEntriesStream.Position - JumpListDestListEntry.EntryPosition;
                        JumpListDestListEntry.RawData = new byte[JumpListDestListEntry.EntrySize];
                        using (MemoryStream JumpListDestListStreamForSave = new MemoryStream(data))
                        {
                            JumpListDestListStreamForSave.Seek(JumpListDestListEntry.EntryPosition, SeekOrigin.Begin);
                            JumpListDestListStreamForSave.Read(JumpListDestListEntry.RawData, 0x0, (int)JumpListDestListEntry.EntrySize);
                        }
                        JumpListDestListEntries.Add(JumpListDestListEntry);
                    }
                    while (JumpListEntriesStream.Position < JumpListEntriesStream.Length);
                }
            }
            catch (Exception e)
            {
                this.ScrubberGUIInst.DebugPrint(e.ToString());
            }
            return JumpListDestListEntries;
        }

        private void ParseJumpList(string JumpListFilePath)
        {
            this.DestListObj = new DestList();
            JumpListFile JumpListFileObj = new JumpListFile
            {
                FilePath = JumpListFilePath,
                FileName = System.IO.Path.GetFileName(JumpListFilePath)
            };
            CompoundFile JumpListFileCompound = new CompoundFile(JumpListFilePath);
            CFStream JumpListDestListStream = JumpListFileCompound.RootStorage.GetStream("DestList");
            JumpListFileObj.DestListSize = JumpListDestListStream.Size;
            List<DestListEntry> JumpListEntries = this.ParseDestList(JumpListDestListStream.GetData());
            JumpListFileObj.DestListEntries = JumpListEntries;
            foreach (DestListEntry JumpListEntry in JumpListEntries)
            {
                CFStream JumpListEntryStream = null;
                try
                {
                    JumpListEntryStream = JumpListFileCompound.RootStorage.GetStream(JumpListEntry.StreamNo);
                    JumpList JumpListObj = new JumpList
                    {
                        Name = JumpListEntry.StreamNo,
                        Size = JumpListEntryStream.GetData().Length,
                        DestListEntry = JumpListEntry
                    };
                    JumpListFileObj.JumpLists.Add(JumpListObj);
                    this.JumpListFiles = JumpListFileObj;
                }
                catch (Exception e)
                {
                    this.ScrubberGUIInst.DebugPrint(e.ToString());
                    return;
                }
            }
            this._jumpListFiles.Add(JumpListFileObj);
            JumpListFileCompound.Close();
        }

        private string ReplaceNulls(string data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return data.Replace("\0", string.Empty);
        }

        private void DebugLogDestListEntry(DestListEntry dle)
        {
            File.WriteAllBytes(dle.StreamNo + ".log", dle.RawData);
        }

        private void DebugLogDestList(byte[] dl, string prefix = "") { 
            File.WriteAllBytes("DestList_" + prefix + "_" + this.JumpListFiles.FileName, dl);
        }
       
        private byte[] CreateDestListBinaryFromDestListObj(JumpListFile JL, bool DeleteUnwanted = false){
            byte[] DestListBinary = new byte[JL.DestListSize];
            using (MemoryStream DestListStream = new MemoryStream(this.DestListObj.Header))
            {
                DestListStream.Read(DestListBinary, 0x0, 0x20);
            }
            if (DeleteUnwanted)
            {
                int ExtraBytes = 0;
                int DestListStreamPosition = 0x20;
                foreach (DestListEntry DestListEntryObj in JL.DestListEntries)
                {
                    if (!DestListEntryObj.PendingDelete)
                    {
                        using (MemoryStream DestListStream = new MemoryStream(DestListEntryObj.RawData))
                        {
                            DestListStream.Read(DestListBinary, DestListStreamPosition, (int)DestListEntryObj.EntrySize);
                        };
                        DestListStreamPosition += (int)DestListEntryObj.EntrySize;
                    }
                    else {
                        ExtraBytes += (int)DestListEntryObj.EntrySize;
                    }
                }
                byte[] NewDestListBinary = new byte[JL.DestListSize - ExtraBytes];
                for (int i = 0; i < NewDestListBinary.Length; i++)
                {
                    NewDestListBinary[i] = DestListBinary[i];
                }
                DestListBinary = NewDestListBinary;
            }
            else
            {
                foreach (DestListEntry DestListEntryObj in JL.DestListEntries)
                {
                    using (MemoryStream DestListStream = new MemoryStream(DestListEntryObj.RawData))
                    {
                        DestListStream.Read(DestListBinary, (int)DestListEntryObj.EntryPosition, (int)DestListEntryObj.EntrySize);
                    };
                }
            }
            return DestListBinary;
        }

        private void DeleteUnwantedJumpListEntries() {
            foreach (JumpList JumpListObj in this.JumpListFiles.JumpLists)
            {
                foreach (string Keyword in this.KeywordsList)
                {
                    if (JumpListObj.DestListEntry.NetBiosName.Contains(Keyword))
                    {
                        JumpListObj.DestListEntry.PendingDelete = true;
                    }
                    else if (JumpListObj.DestListEntry.Data.Contains(Keyword))
                    {
                        JumpListObj.DestListEntry.PendingDelete = true;
                    }
                }
            }
            byte[] dl2 = this.CreateDestListBinaryFromDestListObj(this.JumpListFiles, true);

            CompoundFile JumpListFileCompound = new CompoundFile(this.PathToJumpListFiles, UpdateMode.Update, false, true);
            
            foreach (JumpList JumpListObj in this.JumpListFiles.JumpLists) 
            {
                if (JumpListObj.DestListEntry.PendingDelete) {
                    this.ScrubberGUIInst.DebugPrint("Scrubbing (JumpList): " + JumpListObj.DestListEntry.Data);
                    JumpListFileCompound.RootStorage.Delete(JumpListObj.DestListEntry.StreamNo);
                }
            }

            JumpListFileCompound.RootStorage.GetStream("DestList").SetData(dl2);
            JumpListFileCompound.Commit();
            JumpListFileCompound.Close();
        }

        public static class StreamHelper
        {
            public static string ReadStr(MemoryStream ms, int bytes) { byte[] mybuf = new byte[bytes]; ms.Read(mybuf, 0, bytes); return System.Text.Encoding.ASCII.GetString(mybuf); }
            public static Int64 Read64(MemoryStream ms) { byte[] mybuf = new byte[8]; ms.Read(mybuf, 0, 8); return System.BitConverter.ToInt64(mybuf, 0); }
            public static DateTime ReadDateTime(MemoryStream ms) { byte[] mybuf = new byte[8]; ms.Read(mybuf, 0, 8); Int64 mls = System.BitConverter.ToInt64(mybuf, 0); DateTime myDT = new DateTime(1980, 1, 1); return new DateTime(mls).AddYears(1600)/*.AddMilliseconds((double)12948325676810351)/*myDT.AddMilliseconds(mls)*/; }
            public static Int16 Read16(MemoryStream ms) { byte[] mybuf = new byte[2]; ms.Read(mybuf, 0, 2); return System.BitConverter.ToInt16(mybuf, 0); }
            public static string ReadStrU(MemoryStream ms, int bytes) { byte[] mybuf = new byte[bytes]; ms.Read(mybuf, 0, bytes); return System.Text.Encoding.UTF8.GetString(mybuf); }
        }

    }
}


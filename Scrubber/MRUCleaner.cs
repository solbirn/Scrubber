using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Scrubber
{
    class MRUCleaner
    {
        List<string> KeywordsList;
        List<string> RegLocationsToClean;
        ScrubberGUI ScrubberGUIInst;
        private List<RegistryKey> RegKeysList;

        public MRUCleaner(List<string> KeywordsList, ScrubberGUI ScrubberGUIInst)
        {
            this.KeywordsList = KeywordsList;
            this.ScrubberGUIInst = ScrubberGUIInst;
            RegKeysList = new List<RegistryKey>();
            RegLocationsToClean = new List<string>();

            RegLocationsToClean.Add("software\\microsoft\\windows\\currentversion\\explorer\\recentdocs");
            RegLocationsToClean.Add("software\\microsoft\\windows\\currentversion\\explorer\\typedpaths");
            RegLocationsToClean.Add("Software\\Gabest\\Media Player Classic\\Recent File List");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Regedit");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Regedit\\Favorites");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Paint\\Recent File List");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Wordpad\\Recent File List");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32\\");
            RegLocationsToClean.Add("Software\\Microsoft\\MediaPlayer\\Player\\RecentFileList");
            RegLocationsToClean.Add("Software\\Microsoft\\MediaPlayer\\Player\\RecentURLList");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Map Network Drive MRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Search Assistant\\ACMru\\5603");
            RegLocationsToClean.Add("Software\\Microsoft\\Search Assistant\\ACMru\\5001");
            RegLocationsToClean.Add("Software\\Microsoft\\Search Assistant\\ACMru\\5647");
            RegLocationsToClean.Add("Software\\Microsoft\\Terminal Server Client");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Doc Find Spec MRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FindComputerMRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\PrnPortsMRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StreamMRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft Word\\Settings");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Word\\User MRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft Excel\\Settings");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Excel\\Recent Files");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft FrontPage\\Settings");
            RegLocationsToClean.Add("Software\\Microsoft\\FrontPage\\Explorer\\FrontPage Explorer");
            RegLocationsToClean.Add("Software\\Microsoft\\FrontPage\\Editor\\Recently Used URLs");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft PowerPoint\\Settings");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\PowerPoint\\Recent File List");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft Access\\Settings\\Open\\File Name MRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Common\\Open Find\\Microsoft Access\\Settings\\File New Database\\File Name MRU");
            RegLocationsToClean.Add("Software\\Microsoft\\Office\\15.0\\Access\\Settings");
            //RegLocationsToClean.Add("Software\\Microsoft\\Office\\");
            RegLocationsToClean.Add("Software\\microsoft\\internet explorer\\typedurls");
            //RegLocationsToClean.Add("Software\\microsoft\\internet explorer");
            RegLocationsToClean.Add("Software\\Adobe\\MediaBrowser\\MRU");
            RegLocationsToClean.Add("Software\\Adobe\\Adobe Acrobat\\5.0\\AVGeneral\\cRecentFiles");
            RegLocationsToClean.Add("Software\\Adobe\\Acrobat Reader\\5.0\\AVGeneral\\cRecentFiles");
            RegLocationsToClean.Add("Software\\Adobe\\Adobe Acrobat\\8.0\\AVGeneral\\cRecentFiles");
            RegLocationsToClean.Add("Software\\microsoft\\direct3d\\mostrecentapplication");
            RegLocationsToClean.Add("Software\\microsoft\\windows\\currentversion\\applets\\regedit");

            foreach (string RegLocationToClean in RegLocationsToClean)
            {
                this.GetSubKeys(Registry.CurrentUser.OpenSubKey(RegLocationToClean, true));
            }
            foreach (RegistryKey key in this.RegKeysList)
            {
                this.CleanValues(key);
            }
        }

        public void GetSubKeys(RegistryKey Root)
        {
            try
            {
                foreach (string sk in Root.GetSubKeyNames())
                {
                    try
                    {
                        this.GetSubKeys(Root.OpenSubKey(sk, true));
                    }
                    catch (NullReferenceException)
                    {
                        continue;
                    }
                    catch (System.Security.SecurityException) 
                    {
                        continue;
                    }
                }
                this.RegKeysList.Add(Root);
            }
            catch (NullReferenceException)
            {
                //this.ScrubberGUIInst.DebugPrint("Error: " + e.ToString() + "\r\n" + e.Data);
            }
            return;
        }

        public void CleanValues(RegistryKey Key)
        {

            String[] RegValueNames = Key.GetValueNames();
            Dictionary<string, Object> RegNameValuesDict = new Dictionary<string, Object>();
            foreach (string RegValueName in RegValueNames)
            {
                try
                {
                    RegNameValuesDict.Add(RegValueName, Key.GetValue(RegValueName));
                }
                catch (Exception e)
                {
                    this.ScrubberGUIInst.DebugPrint(e.ToString());
                    continue;
                }
            }

            Dictionary<string, RegistryKey> RegLocationsToCleanDict = new Dictionary<string, RegistryKey>();
            foreach (KeyValuePair<string, Object> RegNameValue in RegNameValuesDict)
            {
                foreach (string Keyword in this.KeywordsList)
                {
                    string regval;
                    if (Key.GetValueKind(RegNameValue.Key) == RegistryValueKind.Binary)
                    {
                        regval = System.Text.Encoding.Unicode.GetString((byte[])RegNameValue.Value);
                    }
                    else if (Key.GetValueKind(RegNameValue.Key) == RegistryValueKind.String)
                    {
                        regval = (string)RegNameValue.Value;
                    }
                    else
                    {
                        regval = "E";
                    }
                    if (regval.Contains(Keyword))
                    {
                        try
                        {
                            if (!RegLocationsToCleanDict.ContainsKey(RegNameValue.Key))
                            {
                                RegLocationsToCleanDict.Add(RegNameValue.Key, Key);
                            }
                            else if (RegLocationsToCleanDict[RegNameValue.Key] != Key)
                            {
                                RegLocationsToCleanDict.Add(RegNameValue.Key, Key);
                            };
                        }
                        catch (Exception e)
                        {
                            this.ScrubberGUIInst.DebugPrint(e.ToString());
                        }
                    };
                    for (int i = 0; i < RegLocationsToCleanDict.Count; i++)
                    {

                    }
                }
            }

            List<string> RegLocationsToCleanList = RegLocationsToCleanDict.Keys.ToList();
            RegLocationsToCleanList.Sort();
            Dictionary<string, RegistryKey> RegLocationsToCleanDictSorted = new Dictionary<string, RegistryKey>();
            for (int le = RegLocationsToCleanList.Count - 1; le >= 0; le--)
            {
                RegLocationsToCleanDictSorted.Add(RegLocationsToCleanList[le], RegLocationsToCleanDict[RegLocationsToCleanList[le]]);
            }

            //Find common Key name for non mru
            string common = "";
            bool isCommon = false;

            if (RegLocationsToCleanList.Count >= 1)
            {
                for (int RegLocationToCleanIndex = 0; RegLocationToCleanIndex < RegLocationsToCleanList[0].Length; RegLocationToCleanIndex++)
                {
                    if (RegLocationsToCleanList[0][RegLocationToCleanIndex] >= '0' && RegLocationsToCleanList[0][RegLocationToCleanIndex] <= '9')
                        break;
                    else
                    {
                        common += RegLocationsToCleanList[0][RegLocationToCleanIndex];
                    }
                }
            }

            if (common.Length > 0)
                isCommon = true;

            foreach (KeyValuePair<string, RegistryKey> RegLocationToClean in RegLocationsToCleanDictSorted)
            {
                Object MRUListEx, MRUList;
                MRUListEx = Key.GetValue("MRUListEx");
                MRUList = Key.GetValue("MRUList");
                Console.WriteLine(RegLocationToClean.Key + ": " + RegLocationToClean.Value + "\n");
                if (MRUListEx != null)
                {
                    try
                    {
                        byte[] MRUEnding = { 0xFF, 0xFF, 0xFF, 0XFF };
                        byte[] MRUListBinary = (byte[])MRUListEx;
                        int reads = (MRUListBinary.Length / 4) - 1;
                        List<byte[]> MRUListBinaryList = new List<byte[]>();
                        Dictionary<string, int> EntryToPosition = new Dictionary<string, int>();

                        for (int i = 0; i < reads; i++)
                        {
                            int MRUListPosition = i * 4;
                            byte[] temp = new byte[4];
                            for (short j = 0; j < 4; j++)
                            {
                                temp[j] = MRUListBinary[MRUListPosition + j];
                            }
                            EntryToPosition.Add(System.BitConverter.ToInt32(temp, 0).ToString(), i);
                            MRUListBinaryList.Add(temp);
                        }

                        string last = (MRUListBinaryList.Count - 1).ToString();
                        if (!(MRUListBinaryList.Count == 1))
                        {
                            object val = Key.GetValue(last);
                            if (val != null) {
                                Key.SetValue(RegLocationToClean.Key, val);
                                DeleteEntry(Key, last);
                            }
                            MRUListBinaryList[EntryToPosition[last]] = MRUListBinaryList[EntryToPosition[RegLocationToClean.Key]];
                            MRUListBinaryList.RemoveAt(EntryToPosition[RegLocationToClean.Key]);

                            MRUListBinaryList.Add(MRUEnding);
                            byte[] newmru = new byte[MRUListBinaryList.Count * 4];
                            int pos = 0;
                            foreach (byte[] mru in MRUListBinaryList)
                            {
                                mru.CopyTo(newmru, pos);
                                pos += 4;
                            }
                            Key.SetValue("MRUListEx", newmru);
                        }
                        else
                        {
                            DeleteEntry(Key, last);
                            DeleteEntry(Key, "MRUListEx");
                        }
                    }
                    catch (Exception e)
                    {
                        this.ScrubberGUIInst.DebugPrint(e.ToString());
                        continue;
                    }
                }
                else if (MRUList != null) 
                {
                    string MRUListString = (string)MRUList;
                    List<char> MRUListCharList = new List<char>(); 
                    Dictionary<char, int> EntryToPosition = new Dictionary<char, int>();
                    for (int i = 0; i < MRUListString.Length ; i++)
                    {
                        EntryToPosition.Add(MRUListString[i], i);
                        MRUListCharList.Add(MRUListString[i]);
                    }

                    string last = "";
                    last += MRUListCharList[MRUListCharList.Count - 1];
                    if (!(MRUListCharList.Count == 1))
                    {
                        Key.SetValue(RegLocationToClean.Key, Key.GetValue(last));
                        DeleteEntry(Key, last);
                        MRUListCharList[EntryToPosition[last[0]]] = MRUListCharList[EntryToPosition[RegLocationToClean.Key[0]]];
                        MRUListCharList.RemoveAt(EntryToPosition[RegLocationToClean.Key[0]]);

                        string NewMRUListString = string.Join("", MRUListCharList.ToArray());
                        Key.SetValue("MRUList", NewMRUListString);
                    }
                    else
                    {
                        DeleteEntry(Key, RegLocationToClean.Key);
                        DeleteEntry(Key, "MRUList");
                    }
                }
                else
                {

                    //Delete Keys
                    DeleteEntry(Key, RegLocationToClean.Key);

                    //Fix names to hide deletes
                    if (isCommon)
                    {
                        int num = 0;
                        List<string> RegValueNamesList = Key.GetValueNames().ToList();
                        RegValueNamesList.Sort();
                        foreach (string RegValueName in RegValueNamesList)
                        {
                            RegistryUtilities.RenameValue(Key, RegValueName, "old" + RegValueName);
                        }
                        foreach (string valname in RegValueNamesList)
                        {
                            num += 1;
                            RegistryUtilities.RenameValue(Key, "old" + valname, common + num.ToString());
                        }
                    }
                    //
                }

            }
        }

        public void DeleteEntry(RegistryKey Key, string ValueName)
        {
            try
            {
                this.ScrubberGUIInst.DebugPrint("Scrubbing (MRU): " + Key.Name);
                Key.DeleteValue(ValueName);
            }
            catch (Exception e)
            {
                this.ScrubberGUIInst.DebugPrint(e.ToString());
                return;
            }
        }

        public void DebugPrint(Dictionary<string, string> DictStrStr)
        {
            foreach (KeyValuePair<string, string> Str in DictStrStr)
            {
               this.ScrubberGUIInst.DebugPrint(Str.Key + ":  " + Str.Value);
            }
        }

    public static class RegistryUtilities
    {
        /// <summary>
        /// Renames a subkey of the passed in registry Key since 
        /// the Framework totally forgot to include such a handy feature.
        /// </summary>
        /// <param name="regKey">The RegistryKey that contains the subkey 
        /// you want to rename (must be writeable)</param>
        /// <param name="subKeyName">The name of the subkey that you want to rename
        /// </param>
        /// <param name="newSubKeyName">The new name of the RegistryKey</param>
        /// <returns>True if succeeds</returns>
        /// 

        public static void RenameValue(RegistryKey parentKey, string valueName, string newValueName)
        {
            object objValue = parentKey.GetValue(valueName);
            RegistryValueKind valKind = parentKey.GetValueKind(valueName);
            parentKey.SetValue(newValueName, objValue, valKind);
            parentKey.DeleteValue(valueName);
        }

        public static bool RenameSubKey(RegistryKey parentKey, 
			string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
            return true;
        }

        public static bool CopyKey(RegistryKey parentKey, 
			string keyNameToCopy, string newKeyName)
        {
            //Create new Key
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);

            //Open the sourceKey we are copying from
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy);

            RegistryUtilities.RecurseCopyKey(sourceKey, destinationKey);

            return true;
        }

        private static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            //copy all the values
            foreach (string valueName in sourceKey.GetValueNames())
            {        
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }
        }
      }
    }
}

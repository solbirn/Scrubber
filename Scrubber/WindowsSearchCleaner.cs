using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Globalization;
    
[assembly: CLSCompliant(true)]
namespace Scrubber.Scrubber
{
    class WindowsSearchCleaner
    {
        //TODO: There are issues with Windows Search scrubbing. Primarily that even deleted items hang around until space is needed ("whitespace") or index is rebuilt.
        //      Haven't decided what to do yet

        // Connection string for Windows Search
        const string connectionString = "Provider=Search.CollatorDSO;Extended Properties=\"Application=Windows\"";
        ScrubberGUI ScrubberGUIInst;

        WindowsSearchCleaner(ScrubberGUI sg)
        {
            this.ScrubberGUIInst = sg;
        }


        // Display the result set recursively expanding chapterDepth deep
        public void DisplayReader(OleDbDataReader myDataReader, ref uint count, uint alignment, int chapterDepth)
        {
            try
            {
                // compute alignment
                StringBuilder indent = new StringBuilder((int) alignment);
                indent.Append(' ', (int) alignment);

                while (myDataReader.Read())
                {
                    // add alignment
                    StringBuilder row = new StringBuilder(indent.ToString());

                    // for all columns
                    for (int i = 0; i < myDataReader.FieldCount; i++)
                    {
                        // null columns
                        if (myDataReader.IsDBNull(i))
                        {
                            row.Append("NULL;");
                        }
                        else
                        {
                            
                            //vector columns
                            object[] myArray = myDataReader.GetValue(i) as object[];
                            if (myArray != null)
                            {
                                DisplayValue(myArray, row);
                            }
                            else
                            {
                                //check for chapter columns from "group on" queries
                                if (myDataReader.GetFieldType(i).ToString() != "System.Data.IDataReader")
                                {
                                    //regular columns are displayed here
                                    row.Append(myDataReader.GetValue(i));
                                }
                                else
                                {
                                    //for a chapter column type just display the colum name
                                    row.Append(myDataReader.GetName(i));
                                }
                            }
                            row.Append(';');
                        }
                    }
                    if (chapterDepth >= 0)
                    {
                        this.ScrubberGUIInst.DebugPrint(row.ToString());
                        count++;
                    }
                    // for each chapter column
                    for (int i = 0; i < myDataReader.FieldCount; i++)
                    {
                        if (myDataReader.GetFieldType(i).ToString() == "System.Data.IDataReader")
                        {
                            OleDbDataReader Reader = myDataReader.GetValue(i) as OleDbDataReader;
                            DisplayReader(Reader, ref count, alignment + 8, chapterDepth - 1);
                        }
                    }
                }
            }
            finally
            {
                myDataReader.Close();
                myDataReader.Dispose();
            }
        }

        // display the value recursively
        static void DisplayValue(object value, StringBuilder sb)
        {
            if (value != null)
            {
                if (value.GetType().IsArray)
                {
                    sb.Append("[");
                    bool first = true;

                    // display every element
                    foreach (object subval in value as Array)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append("; ");
                        }
                        DisplayValue(subval, sb);
                    }

                    sb.Append("]");
                }
                else
                {
                    if (value.GetType() == typeof(double))
                    {
                        // Normal numeric formats round, but we want to report the actual round trip format
                        sb.AppendFormat("{0:r}", value);
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }
            }
        }

        // Run a query and display the rowset up to chapterDepth deep
        public void ExecuteQuery(string query, int chapterDepth)
        {
            OleDbDataReader myDataReader = null; 
            OleDbConnection myOleDbConnection  = new OleDbConnection(connectionString);
            OleDbCommand myOleDbCommand  = new OleDbCommand(query, myOleDbConnection);
            try
            {
                this.ScrubberGUIInst.DebugPrint("Query=" + query);
                myOleDbConnection.Open();
                myDataReader = myOleDbCommand.ExecuteReader();
                if (!myDataReader.HasRows)
                {
                    this.ScrubberGUIInst.DebugPrint("Query returned 0 rows!");
                    return;
                }
                uint count = 0;
                DisplayReader(myDataReader, ref count, 0, chapterDepth);
                this.ScrubberGUIInst.DebugPrint("Rows+Chapters=" + count);
            }
            catch (System.Data.OleDb.OleDbException oleDbException)
            {
                this.ScrubberGUIInst.DebugPrint(System.String.Format("Got OleDbException, error code is 0x{0:X}L", oleDbException.ErrorCode));
                this.ScrubberGUIInst.DebugPrint("Exception details:");
                for (int i = 0; i < oleDbException.Errors.Count; i++)
                {
                    this.ScrubberGUIInst.DebugPrint("\tError " + i.ToString(CultureInfo.CurrentCulture.NumberFormat) + "\n" +
                                      "\t\tMessage: " + oleDbException.Errors[i].Message + "\n" +
                                      "\t\tNative: " + oleDbException.Errors[i].NativeError.ToString(CultureInfo.CurrentCulture.NumberFormat) + "\n" +
                                      "\t\tSource: " + oleDbException.Errors[i].Source + "\n" +
                                      "\t\tSQL: " + oleDbException.Errors[i].SQLState + "\n");
                }
                this.ScrubberGUIInst.DebugPrint(oleDbException.ToString());
                Console.ReadKey();
            }
            finally
            {
                // Always call Close when done reading.
                if (myDataReader != null)
                {
                    myDataReader.Close();
                    myDataReader.Dispose();
                }
                // Close the connection when done with it.
                if (myOleDbConnection.State == System.Data.ConnectionState.Open)
                {
                    myOleDbConnection.Close();
                }
            }
        }

        /*static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            int chapterDepth = 1000000000;
            try
            {
                string query = args[0];
                if (args.Length == 2)
                {
                    chapterDepth = Convert.ToInt32(args[1], CultureInfo.CurrentCulture.NumberFormat);
                }
                this.ExecuteQuery(query, chapterDepth);
            }
            catch (Exception e)
            {
                this.ScrubberGUIInst.DebugPrint(e);
                this.ScrubberGUIInst.DebugPrint();
            }
        }*/
    }
}
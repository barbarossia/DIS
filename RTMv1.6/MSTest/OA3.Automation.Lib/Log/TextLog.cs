using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WPFAutomation.Core;

namespace OA3.Automation.Lib.Log
{
    /// <summary>
    /// Record log message into a txt file
    /// </summary>
    public class TextLog
    {
        //The path where thd log file created
        public static string logFilePath = "LOG_" + Helper.GetDateTimeString() + ".txt";

        public static void LogMessage(string message)
        {
            try
            {
                using (FileStream fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToString() + " : " + message);
                    sw.Close();
                    Console.WriteLine(DateTime.Now.ToString() + " : " + message);
                }
            }
            catch (Exception)
            {
                throw;
            }
          
        }
    }
}

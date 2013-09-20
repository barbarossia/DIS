using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OA3.Automation.Lib.Log
{
    public interface ILogBase
    {
        /// <summary>
        /// Add log to DB/File
        /// </summary>
        /// <param name="message"></param>
        void LogMessage(string message);
    }
}

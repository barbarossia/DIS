using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib.Log;
using OA3.Automation.Lib.KMT;

namespace OA3.Automation.Lib
{
    /// <summary>
    /// Verification Class to assert test result
    /// </summary>
    public class Verification
    {
        /// <summary>
        /// Verification method to KMT Test Case
        /// </summary>
        /// <param name="expectResult"></param>
        /// <param name="methodName"></param>
        public static void AssertKMTResponse(bool expectResult,string methodName)
        {

            if (expectResult == true)
            {
                TextLog.LogMessage(methodName + ":    /****** Successfully ******/");
            }
            else
            {
                TextLog.LogMessage(methodName + ":    /######## Fail #######/");
            }
            Assert.AreEqual(true,expectResult);
        }
    }
}

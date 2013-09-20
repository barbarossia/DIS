using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;


namespace OA3.Automation.Lib
{
    public class Helper
    {
        /// <summary>
        /// Get log integer represent the date time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetDateTimeString()
        {
            DateTime time = DateTime.Now;
            return time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString();
        }

        /// <summary>
        /// Convert a XmlDocument To String
        /// </summary>
        /// <param name="xDoc"></param>
        /// <returns></returns>
        public static string XmlToString(XmlDocument xDoc)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter tx = new XmlTextWriter(sw);
                xDoc.WriteTo(tx);
                return sw.ToString();
            }
        }
    }
}

//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DIS.Common.Utility
{
    /// <summary>
    /// Message logger
    /// </summary>
    public class MessageLogger
    {
        /// <summary>
        /// Log count per file
        /// </summary>
        public const int LogsPerFile = 500;
        /// <summary>
        /// Category name of system logs
        /// </summary>
        public const string SystemCategoryName = "System";
        /// <summary>
        /// Category name of operation logs
        /// </summary>
        public const string OperationCategoryName = "Operation";

        /// <summary>
        /// Get current method name
        /// </summary>
        /// <returns></returns>
        public static string GetMethodName()
        {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }

        /// <summary>
        /// Get the title of operation logs
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetOperationLogTitle(string userName)
        {
            string title = "KMT";
            if (!string.IsNullOrEmpty(userName))
                title = string.Format("{0} - {1}", title, userName);
            return title;
        }

        /// <summary>
        /// Log user operations
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public static void LogOperation(string userName, string message)
        {
            LogMessage(GetOperationLogTitle(userName), message, OperationCategoryName,
                TraceEventType.Information);
        }

        /// <summary>
        /// Log system errors
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void LogSystemError(string title, string message)
        {
            LogSystemRunning(title, message, TraceEventType.Error);
        }

        /// <summary>
        /// Log running infomation of system
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="eventType"></param>
        public static void LogSystemRunning(string title, string message, 
            TraceEventType eventType = TraceEventType.Information)
        {
            LogMessage(title, message, SystemCategoryName, eventType);
        }

        private static void LogMessage(string title, string message, string category, 
            TraceEventType eventType)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Title = title;
            logEntry.Message = message;
            logEntry.Categories.Add(category);
            logEntry.Severity = eventType;
            logEntry.EventId = 100;
            logEntry.Priority = 0;
            logEntry.TimeStamp = DateTime.UtcNow;
            logEntry.MachineName = Environment.MachineName;
            Logger.Write(logEntry);
        } 
    }
}

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

using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using DIS.Common.Utility;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionHandler.HandleException(e.Exception);
            e.Exception.ShowDialog();
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = KmtConstants.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = KmtConstants.CurrentCulture;
        }
    }
}

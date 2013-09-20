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
using System.Linq;
using System.Threading;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using System.Windows;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for Login View
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Priviate & Protected member variables

        private const string loginIDPropertyName = "LoginId";
        private const string passwordPropertyName = "Password";
        private DelegateCommand loginCommand;
        private DelegateCommand cancelCommand;
        private bool isBusy;

        #endregion

        public LoginViewModel() 
        {
        }

        #region Public Properties

        public string LoginTitle {
            get {
                return string.Format("{0} - {1}", MergedResources.Common_Login, KmtConstants.InventoryName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Property for displaying Progress Bar
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Command used on clicking Cancel button
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(Cancel);
                }
                return cancelCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Login button
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new DelegateCommand(Login,
                        () => { return !string.IsNullOrEmpty(LoginId) && !string.IsNullOrEmpty(Password); });
                }
                return loginCommand;
            }
        }

        #endregion

        #region Private & Protected methods

        private void Login()
        {
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    IUserProxy userProxy = new UserProxy();
                    KmtConstants.LoginUser = userProxy.Login(LoginId, Password);
                    if (KmtConstants.LoginUser != null)
                    {
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, "Logged in.");

                        IConfigProxy configProxy = new ConfigProxy(KmtConstants.LoginUser);
                        KmtConstants.OldTimeline = configProxy.GetOldTimeline();

                        IHeadQuarterProxy headQuarterProxy = new HeadQuarterProxy();

                        var headQuarters = headQuarterProxy.GetHeadQuarters(KmtConstants.LoginUser);
                        if (headQuarters.Count > 0)
                        {
                            KmtConstants.CurrentHeadQuarter = headQuarters.Single(
                               hq => hq.UserHeadQuarters.First().IsDefault);
                        }

                        Dispatch(() =>
                        {
                            Thread.CurrentThread.CurrentUICulture = KmtConstants.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = KmtConstants.CurrentCulture;
                            MainWindow mainWindow = new MainWindow();
                            View.Close();
                            mainWindow.Show();
                        });
                    }
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex);
                }
            });
        }

        /// <summary>
        /// Close the window
        /// </summary>
        private void Cancel()
        {
            View.Close();
        }

        #endregion
    }
}

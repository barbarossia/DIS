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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;
using System.Windows;
using System;
using DIS.Common.Utility;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportKeysViewModel : TemplateViewModelBase
    {
        #region Contructor

        /// <summary>
        /// View Model class fo rReport Keys View
        /// </summary>
        /// <param name="keyProxy"></param>
        public ReportKeysViewModel(IKeyProxy keyProxy)
            : base(keyProxy)
        {
            base.WindowTitle = MergedResources.MainWindow_ReportKeys;

            Initialize();
            this.SearchControlVM.KeyTypesVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string SendFailedText
        {
            get
            {
                return sendFailedText;
            }
            set
            {
                sendFailedText = value;
                RaisePropertyChanged("SendFailedText");
            }
        }

        #endregion

        #region Private Members

        private string sendFailedText;

        private void Initialize()
        {
            base.InitView();
            base.SearchAllKeys();
        }

        #endregion

        #region Override Members

        /// <summary>
        /// Get default keys for report
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchBoundKeysToReport(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        /// <summary>
        /// Search key for report by groups
        /// </summary>
        protected override void SearchKeyGroups()
        {
            KeyGroups = new ObservableCollection<KeyGroupModel>(base.keyProxy.SearchBoundKeyGroupsToReport(KeySearchCriteria).ToKeyGroupModel());
        }

        /// <summary>
        /// Execute keys for report
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            if (base.TabIndex == KmtConstants.FirstTab)
                result = base.keyProxy.SendBoundKeys(base.KeyGroups.Select(k => k.KeyGroup).Where(k => k.Quantity > 0).ToList());
            else
                result = base.keyProxy.SendBoundKeys(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList());

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            base.SummaryText = string.Format(MergedResources.ReportKeysViewModel_ReportSuccess,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
            if (base.KeyOperationResults.Any(r => r.Failed))
            {
                this.SendFailedText = ResourcesOfR6.Common_ReportFailed;
            }
        }

        #endregion
    }
}

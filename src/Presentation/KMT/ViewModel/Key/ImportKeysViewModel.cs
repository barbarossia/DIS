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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;
using DIS.Presentation.KMT.Views.Key.DuplicateKeysView;
using Microsoft.Win32;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ImportKeysViewModel : TemplateViewModelBase
    {
        #region Private fields
        private ISubsidiaryProxy ssProxy;
        private IConfigProxy configProxy;
        private IHeadQuarterProxy headquarterProxy;

        private Constants.ExportType importType;
        private string fileName;
        private string rbKeyContent;
        private string resultMsg;

        private bool isKeysChecked;
        private bool isKeysReportChecked;
        private bool isToolKeyChecked;
        private bool isCBRChecked;
        private bool isDuplicateCBRChecked;
        private bool isReturnAckChecked;
        private bool isCheckFileSignature = false;

        private bool rbKeyVisibility;
        private bool rbKeyReportVisibility;
        private bool rbToolKeyVisibility;
        private bool rbCBRVisibility;
        private bool rbDuplicateCBRVisibility;
        private bool checkFileSignatureVisibility = false;

        private bool isDlSEnable;
        private bool isUlSEnable;
        private bool isMSEnable;

        private Visibility handleVisibility;

        private ICommand browseCommand;
        private ICommand handleCommand;
        #endregion

        #region Constructors

        public ImportKeysViewModel() : this(null, null, null, null) { }

        public ImportKeysViewModel(IKeyProxy keyProxy, ISubsidiaryProxy ssProxy, IConfigProxy configProxy, IHeadQuarterProxy hqProxy)
            : base(keyProxy)
        {
            this.ssProxy = ssProxy;
            this.headquarterProxy = hqProxy;
            this.configProxy = configProxy;
            base.WindowTitle = MergedResources.Import_WinTit;
            InitializeCollections();
            InitRbEnable();
        }

        #endregion

        #region Binding properties

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public string RbKeyContent
        {
            get { return rbKeyContent; }
            set
            {
                rbKeyContent = value;
                RaisePropertyChanged("RbKeyContent");
            }
        }

        public string ResultMsg
        {
            get { return resultMsg; }
            set
            {
                resultMsg = value;
                RaisePropertyChanged("ResultMsg");
            }
        }

        public bool IsKeysChecked
        {
            get { return isKeysChecked; }
            set
            {
                isKeysChecked = value;
                if (isKeysChecked)
                {
                    this.importType = Constants.ExportType.FulfilledKeys;
                    //CheckFileSignatureVisibility = true;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsKeysChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsKeysReportChecked
        {
            get { return isKeysReportChecked; }
            set
            {
                isKeysReportChecked = value;
                if (isKeysReportChecked)
                {
                    this.importType = Constants.ExportType.ReportKeys;
                    //CheckFileSignatureVisibility = true;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsKeysReportChecked");
            }
        }

        public bool IsToolKeyChecked
        {
            get { return isToolKeyChecked; }
            set
            {
                isToolKeyChecked = value;
                if (isToolKeyChecked)
                {
                    this.importType = Constants.ExportType.ToolKeys;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsToolKeyChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCBRChecked
        {
            get { return isCBRChecked; }
            set
            {
                isCBRChecked = value;
                if (isCBRChecked)
                {
                    this.importType = Constants.ExportType.CBR;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsCBRChecked");
            }
        }

        public bool IsDuplicateCBRChecked
        {
            get { return isDuplicateCBRChecked; }
            set
            {
                isDuplicateCBRChecked = value;
                if (isDuplicateCBRChecked)
                {
                    this.importType = Constants.ExportType.DuplicateCBR;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsDuplicateCBRChecked");
            }
        }

        public bool IsReturnAckChecked
        {
            get { return isReturnAckChecked; }
            set
            {
                isReturnAckChecked = value;
                if (IsReturnAckChecked)
                {
                    this.importType = Constants.ExportType.ReturnAck;
                    CheckFileSignatureVisibility = false;
                }
                RaisePropertyChanged("IsReturnAckChecked");
            }
        }

        public bool IsCheckFileSignature
        {
            get { return isCheckFileSignature; }
            set
            {
                isCheckFileSignature = value;
                RaisePropertyChanged("IsCheckFileSignature");
            }
        }

        public bool IsDlSEnable
        {
            get { return isDlSEnable; }
            set
            {
                isDlSEnable = value;
                RaisePropertyChanged("IsDlSEnable");
            }
        }

        public bool IsUlSEnable
        {
            get { return isUlSEnable; }
            set
            {
                isUlSEnable = value;
                RaisePropertyChanged("IsUlSEnable");
            }
        }

        public bool IsMSEnable
        {
            get { return isMSEnable; }
            set
            {
                isMSEnable = value;
                RaisePropertyChanged("IsMSEnable");
            }
        }

        public Visibility HandleVisibility
        {
            get { return handleVisibility; }
            set
            {
                handleVisibility = value;
                RaisePropertyChanged("HandleVisibility");
            }
        }

        public bool RbKeyVisibility
        {
            get { return rbKeyVisibility; }
            set
            {
                rbKeyVisibility = value;
                RaisePropertyChanged("RbKeyVisibility");
            }
        }

        public bool RbKeyReportVisibility
        {
            get { return rbKeyReportVisibility; }
            set
            {
                rbKeyReportVisibility = value;
                RaisePropertyChanged("RbKeyReportVisibility");
            }
        }

        public bool RbToolKeyVisibility
        {
            get { return rbToolKeyVisibility; }
            set
            {
                rbToolKeyVisibility = value;
                RaisePropertyChanged("RbToolKeyVisibility");
            }
        }

        public bool RbCBRVisibility
        {
            get { return rbCBRVisibility; }
            set
            {
                rbCBRVisibility = value;
                RaisePropertyChanged("RbCBRVisibility");
            }
        }

        public bool RbDuplicateCBRVisibility
        {
            get { return rbDuplicateCBRVisibility; }
            set
            {
                rbDuplicateCBRVisibility = value;
                RaisePropertyChanged("RbDuplicateCBRVisibility");
            }
        }

        public bool CheckFileSignatureVisibility
        {
            get { return checkFileSignatureVisibility; }
            set
            {
                checkFileSignatureVisibility = value;
                RaisePropertyChanged("CheckFileSignatureVisibility");
            }
        }

        public string ULSImport_keyTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Import_KeysTxtTpi;
                else
                    return MergedResources.Import_KeysTxt;
            }
        }


        public string DLSImport_ReportTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Import_ReportKeysTxtTpi;
                else
                    return MergedResources.Import_ReportKeysTxt;
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                if (browseCommand == null)
                    browseCommand = new DelegateCommand(() => { ChooseImportFile(); });
                return browseCommand;
            }
        }

        public ICommand HandleCommand
        {
            get
            {
                if (handleCommand == null)
                    handleCommand = new DelegateCommand(() => { HandleError(); });
                return handleCommand;
            }
        }


        private DelegateCommand nextCommand;

        public override ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                    nextCommand = new DelegateCommand(() => { GoToNextPage(); },
                    () => { return IsKeysChecked || IsKeysReportChecked || IsCBRChecked || IsToolKeyChecked || IsReturnAckChecked || IsDuplicateCBRChecked; });
                return nextCommand;
            }
        }

        private DelegateCommand executeCommand;

        public override ICommand ExecuteCommand
        {
            get
            {
                if (executeCommand == null)
                    executeCommand = new DelegateCommand(() => { Execute(); },
                         () => { this.IsExecuteButtonEnable = true; return !string.IsNullOrEmpty(FileName) && File.Exists(FileName); });
                return executeCommand;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// init UI display
        /// </summary>
        private void InitializeCollections()
        {
            base.TabIndex = KmtConstants.SecondTab;
            if (KmtConstants.IsFactoryFloor)
            {
                this.RbKeyVisibility = true;
                this.RbKeyReportVisibility = false;
                this.RbToolKeyVisibility = true;
                this.RbCBRVisibility = false;
                this.RbDuplicateCBRVisibility = false;
                this.InitView();
            }
            else
            {
                if (KmtConstants.IsTpiCorp)
                {
                    this.RbKeyVisibility = true;
                    if (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                    {
                        this.RbKeyVisibility = true;
                        this.RbDuplicateCBRVisibility = false;
                    }
                    else
                    {
                        this.RbKeyVisibility = false;
                        this.RbDuplicateCBRVisibility = true;
                    }
                }
                else
                {
                    this.RbKeyVisibility = false;
                    this.RbDuplicateCBRVisibility = true;
                }

                this.RbKeyReportVisibility = true;
                this.RbToolKeyVisibility = false;
                this.RbCBRVisibility = true;
                this.InitView();
            }
        }

        /// <summary>
        /// init rbcheck is enable
        /// </summary>
        private void InitRbEnable()
        {
            List<HeadQuarter> headquarters = headquarterProxy.GetHeadQuarters();
            if (headquarters != null && headquarters.Count > 0)
                IsUlSEnable = true;
            else
                IsUlSEnable = false;
            List<Subsidiary> subsidarys = ssProxy.GetSubsidiaries();
            if (subsidarys != null && subsidarys.Count > 0)
                IsDlSEnable = true;
            else
                IsDlSEnable = false;
            IsMSEnable = configProxy.GetIsMsServiceEnabled();

            if (!IsUlSEnable)
                RbKeyVisibility = false;
            if (!IsDlSEnable)
                RbKeyReportVisibility = false;
            if (KmtConstants.IsTpiCorp)
            {
                if (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                    RbCBRVisibility = false;
                else
                    RbCBRVisibility = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitView()
        {
            base.InitView();
            this.IsNextButtonVisible = true;
            this.IsExecuteButtonVisible = false;
        }

        protected override List<KeyInfoModel> SearchKeys() { return null; }

        protected override void SearchKeyGroups() { }

        protected override bool ValidateKeyGroups() { return true; }

        /// <summary>
        /// validate import file
        /// </summary>
        protected override bool ValidateKeys()
        {
            if (!File.Exists(FileName))
            {
                MessageBox.Show(MergedResources.ImportKey_FileNotExists, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// exec import file keys
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();
            switch (this.importType)
            {
                case Constants.ExportType.FulfilledKeys:
                    result = keyProxy.ImportULSFulfilledKeys(FileName, KmtConstants.CurrentHeadQuarter, !this.IsCheckFileSignature);
                    break;
                case Constants.ExportType.ReportKeys:
                    result = keyProxy.ImportDLSBoundKeys(FileName, !this.IsCheckFileSignature);
                    break;
                case Constants.ExportType.ToolKeys:
                    result = keyProxy.ImportToolKey(FileName);
                    break;
                case Constants.ExportType.CBR:
                    result = keyProxy.ImportCbr(FileName);
                    break;
                case Constants.ExportType.DuplicateCBR:
                    result = keyProxy.ImportCbr(this.FileName,true);
                    break;
                case Constants.ExportType.ReturnAck:
                    result = keyProxy.ImportReturnAckKeys(this.FileName);
                    break;
                default:
                    break;
            }
            KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            SummaryText = string.Format("{0} was import.", FileName);
            ResultMsg = string.Format(MergedResources.ImportKey_ResultMsg, result.Count(r => !r.Failed).ToString(), result.Count(r => r.Failed).ToString());
        }

        /// <summary>
        /// next operation
        /// </summary>
        protected override void GoToNextPage()
        {
            if (this.CurrentPageIndex == 0)
                base.GoToNextPage();
        }

        /// <summary>
        /// previous operation
        /// </summary>
        protected override void GoToPreviousPage()
        {
            if (this.CurrentPageIndex == 1)
                base.GoToPreviousPage();
            FileName = string.Empty;
        }

        /// <summary>
        /// final page 
        /// </summary>
        protected override void GoToFinalPage()
        {
            base.GoToFinalPage();
            if (KeyOperationResults.Where(k => k.FailedType == KeyErrorType.ToBeConfirmed).Count() > 0 || this.importType == Constants.ExportType.DuplicateCBR)
                this.HandleVisibility = Visibility.Visible;
        }

        /// <summary>
        /// choose import file location
        /// </summary>
        private void ChooseImportFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MergedResources.Common_XmlFilter;
            ofd.Title = MergedResources.ChooseImportFile;
            if (ofd.ShowDialog() == true)
                FileName = ofd.FileName;
        }

        /// <summary>
        /// handle duplicate keys
        /// </summary>
        private void HandleError()
        {
            DuplicateKeys duplicateWindow = new DuplicateKeys(keyProxy, ssProxy);
            duplicateWindow.Owner = this.View;
            duplicateWindow.ShowDialog();
        }

        #endregion
    }
}

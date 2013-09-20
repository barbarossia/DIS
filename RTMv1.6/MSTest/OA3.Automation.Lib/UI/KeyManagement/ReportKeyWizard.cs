using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
   public class ReportKeyWizard : CommonWindow
    {
        private AutomationElement reportEle;
        public ReportKeyWizard(AutomationElement parent,string name):base(parent,name) {
            reportEle = MainElement;
        }

        #region Select
        /// <summary>
        /// Product key list for Report Window
        /// </summary>
        private TabControl _tab;
        public TabControl Tab
        {
            get
            {
                _tab = new TabControl(reportEle, "TabControl");
                return _tab;
            }
        }

        /// <summary>
        /// DataGrid by Quantity
        /// </summary>
        private CommDataGrid _dataGridByQuantity;
        public CommDataGrid DataGridByQuantity
        {
            get
            {
                _dataGridByQuantity = new CommDataGrid(reportEle, "dgByQuantityKeys");
                return _dataGridByQuantity;
            }
        }

        /// <summary>
        /// Data Grid By ID
        /// </summary>
        private CommDataGrid _dataGridByid;
        public CommDataGrid DataGridByID
        {
            get
            {
                _dataGridByid = new CommDataGrid(reportEle, "dgByKeys");
                return _dataGridByid;
            }
        }

       /// <summary>
       /// The dataGrid on the Summary pageK
       /// </summary>
        private DataGrid _dataGrid;
        public DataGrid DataGrid
        {
            get {
                _dataGrid = new DataGrid(reportEle, "dgAssignedKeys");
                return _dataGrid; }
          
        }

        #endregion 


       /// <summary>
       /// Error message
       /// </summary>
        private MessageBox _messageBoxError;
        public MessageBox MessageBoxError
        {
            get {
                _messageBoxError = new MessageBox(reportEle, "#32770");
                return _messageBoxError; }
        }

        #region  Button
        /// <summary>
        /// OK button
        /// </summary>
        private Button _OKbutton;
        public Button OKButton
        {
            get
            {
                _OKbutton = new Button(reportEle, "btnOK");
                return _OKbutton;
            }
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        private Button _cancelbutton;
        public Button CancelButton
        {
            get
            {
                _cancelbutton = new Button(reportEle, "btnCancel");
                return _cancelbutton;
            }
        }

        /// <summary>
        /// Close Button
        /// </summary>
        private Button _closebutton;
        public Button CloseButton
        {
            get
            {
                _closebutton = new Button(reportEle, "btnFinish");
                return _closebutton;
            }
        }
        #endregion
    }
}

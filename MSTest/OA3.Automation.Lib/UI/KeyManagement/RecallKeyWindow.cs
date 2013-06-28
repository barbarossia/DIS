using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
   public class RecallKeyWindow : CommonWindow
    {
        private AutomationElement recallEle;
        public RecallKeyWindow(AutomationElement parent, string name)
            : base(parent, name)
        {
            recallEle = MainElement;
        }

        /// <summary>
        /// Product key list for Recall Window
        /// </summary>
        private TabControl _tab;
        public TabControl Tab
        {
            get
            {
                _tab = new TabControl(recallEle, "TabControl");
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
                _dataGridByQuantity = new CommDataGrid(recallEle, "dgByQuantityKeys");
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
                _dataGridByid = new CommDataGrid(recallEle, "dgByKeys");
                return _dataGridByid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Label _libararySummary;
        public Label LibararySummary
        {
            get
            {
                _libararySummary = new Label(recallEle, "lblSummary");
                return _libararySummary;
            }
        }

       /// <summary>
       /// DataGrid of Summary Page.
       /// </summary>
        private DataGrid _dataGrid;
        public DataGrid DataGrid
        {
            get {
                _dataGrid = new DataGrid(recallEle, "dgAssignedKeys");
                return _dataGrid; }
         
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
                _OKbutton = new Button(recallEle, "btnOK");
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
                _cancelbutton = new Button(recallEle, "btnCancel");
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
                _closebutton = new Button(recallEle, "btnFinish");
                return _closebutton;
            }
        }
        #endregion

    }
}

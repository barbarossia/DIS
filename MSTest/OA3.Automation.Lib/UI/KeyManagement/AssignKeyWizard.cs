using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
    public class AssignKeyWizard : CommonWindow
    {
        private AutomationElement assignEle;
        public AssignKeyWizard(AutomationElement parentEle, string Name)
            : base(parentEle, Name)
        {
            assignEle = MainElement;
        }

        #region Select
        /// <summary>
        /// Product key list for Assign Window
        /// </summary>
        private TabControl _tab;
        public TabControl Tab
        {
            get
            {
                _tab = new TabControl(assignEle, "TabControl");
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
                _dataGridByQuantity = new CommDataGrid(assignEle, "dgByQuantityKeys");
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
                _dataGridByid = new CommDataGrid(assignEle, "dgByKeys");
                return _dataGridByid;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private Label _libararySummary;
        public Label LibararySummary
        {
            get {
                _libararySummary = new Label(assignEle, "lblSummary");
                return _libararySummary; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DataGrid _dataGridSummary;
        public DataGrid DataGridSummary
        {
            get
            {
                _dataGridSummary = new DataGrid(assignEle, "dgAssignedKeys");
                return _dataGridSummary;
            }
        }

        /// <summary>
        /// Target Inventory
        /// </summary>
        private ComboBox _comboBox;
        public ComboBox ComboBox
        {
            get
            {
                _comboBox = new ComboBox(assignEle, "drpTpi");
                return _comboBox;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private MessageBox _msgError;
        public MessageBox MsgError
        {
            get
            {
                _msgError = new MessageBox(assignEle, "#32770");
                return _msgError;
            }

        }
        /// <summary>
        /// OK button
        /// </summary>
        private Button _OKbutton;
        public Button OKButton
        {
            get
            {
                _OKbutton = new Button(assignEle, "btnOK");
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
                _cancelbutton = new Button(assignEle, "btnCancel");
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
                _closebutton = new Button(assignEle, "btnFinish");
                return _closebutton;
            }
        }



    }
}

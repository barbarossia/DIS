using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;
using System.Windows.Automation;
using OA3.Automation.Lib.UI.Windows;
using OA3.Automation.Lib.Log;

namespace OA3.Automation.Lib.UI
{
    public class MainWindow : BaseWindow
    {
        private AutomationElement TempEle;
        public MainWindow(AutomationElement autoElement, string title)
            : base(autoElement, title)
        {
            TempEle = MainElement;
            TextLog.LogMessage("Loaded to the "+title+" Window!");
        }

        /// <summary>
        /// Search button
        /// </summary>
        private Button _searchButton;
        public Button SearchButton
        {
            get
            {
                _searchButton = new Button(TempEle, "btnSearch");
                return _searchButton;
            }
        }

        /// <summary>
        /// Display User role
        /// </summary>
        private Label _displayUser;
        public Label DisplayUser
        {
            get
            {
                _displayUser = new Label(TempEle, "Text");
                return _displayUser;
            }

        }

        /// <summary>
        /// Ribbon control
        /// </summary>
        private DISRibbon _ribbon;
        public DISRibbon Ribbon
        {
            get
            {
                _ribbon = new DISRibbon(TempEle, "Ribbon");
                return _ribbon;
            }
        }


        /// <summary>
        /// Key list 
        /// </summary>
        private DataGrid _datagridKey;
        public DataGrid DatagridKeyList
        {
            get
            {
                _datagridKey = new DataGrid(TempEle, "grdKeyList");
                return _datagridKey;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private DataGrid _KeyDetail;
        public DataGrid KeyDetail
        {
            get
            {
                _KeyDetail = new DataGrid(TempEle, "dgKeyDetails");
                return _KeyDetail;
            }

        }

        #region Search Condition
        /// <summary>
        /// Search condition "Status"
        /// </summary>
        private ComboBox _status;

        public ComboBox Status
        {
            get
            {
                _status = new ComboBox(TempEle, "cboKeyStatus");
                return _status;
            }
            set { _status = value; }
        }

        /// <summary>
        /// Search condition "Assign To"
        /// </summary>
        private ComboBox _AssignTo;

        public ComboBox AssignTo
        {
            get { return _AssignTo; }
            set { _AssignTo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private TextBox _OEMP0Number;

        public TextBox OEMP0Number
        {
            get { return _OEMP0Number; }
            set { _OEMP0Number = value; }
        }


        private TextBox _callOffNumber;

        public TextBox CallOffNumber
        {
            get { return _callOffNumber; }
            set { _callOffNumber = value; }
        }


        private TextBox _orderNumber;

        public TextBox OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }


        private TextBox _MSpartNumber;

        public TextBox MSpartNumber
        {
            get { return _MSpartNumber; }
            set { _MSpartNumber = value; }
        }


        private TextBox _OEMpartNumber;

        public TextBox OEMpartNumber
        {
            get { return _OEMpartNumber; }
            set { _OEMpartNumber = value; }
        }
        #endregion

        #region Key List
        /// <summary>
        /// 
        /// </summary>
        private ComboBox _comboboxPage;
        public ComboBox ComboboxPage
        {
            get
            {
                _comboboxPage = new ComboBox(TempEle, "cmbPageNo");
                return _comboboxPage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private TextBlock _textBlockTotal;
        public TextBlock TextBlockTotal
        {
            get
            {
                try
                {
                    _textBlockTotal = new TextBlock(TempEle, "txtTotalCount");
                    return _textBlockTotal;
                }
                catch (Exception)
                {
                    return null;
                }
               
            }
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        private MessageBox _MsgBoxGetKeys;
        public MessageBox MsgBoxGetKeys
        {
            get
            {
                _MsgBoxGetKeys = new MessageBox(TempEle, "#32770");
                return _MsgBoxGetKeys;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ProgressBar _progressBar;
        public ProgressBar ProgressBar
        {
            get
            {
                _progressBar = new ProgressBar(TempEle, "progressBar");
                return _progressBar;
            }
        }
    }
}

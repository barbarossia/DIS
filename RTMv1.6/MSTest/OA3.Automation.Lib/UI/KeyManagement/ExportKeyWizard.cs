using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core.Controls;
using System.Windows.Automation;
using WPFAutomation.Core;

namespace OA3.Automation.Lib.UI.Windows
{
   public class ExportKeyWizard : CommonWindow
    {  
        private AutomationElement exportEle;
        public ExportKeyWizard(AutomationElement parentEle, string title)
            : base(parentEle, title)
        {
            exportEle = MainElement;
        }

        #region Radio Button list
        /// <summary>
        /// Export keys radio button
        /// </summary>
        private RadioButton _exportKeys;
        public RadioButton ExportKeys
        {
            get {
                _exportKeys = new RadioButton(exportEle, "rbkeys");
                return _exportKeys; }
        }

        /// <summary>
        /// Re-Export keys radio button
        /// </summary>
        private RadioButton _RexportKeys;
        public RadioButton RexportKeys
        {
            get
            {
                _RexportKeys = new RadioButton(exportEle, "rbRekeys");
                return _RexportKeys;
            }
        }

       /// <summary>
       /// 
       /// </summary>
        private RadioButton _exportCBR;
        public RadioButton ExportCBR
        {
            get {
                _exportCBR = new RadioButton(exportEle, "rbCBR");
                return _exportCBR; }
        }

       /// <summary>
       /// 
       /// </summary>
        private RadioButton _exportForOA3;
        public RadioButton ExportForOA3
        {
            get {
                _exportForOA3 = new RadioButton(exportEle, "RbToolKey");
                return _exportForOA3; }
           
        }

       /// <summary>
       /// 
       /// </summary>
        private RadioButton _exportDupCBR;
        public RadioButton ExportDupCBR
        {
            get {
                _exportDupCBR = new RadioButton(exportEle, "rbReCBR");
                return _exportDupCBR; }
           
        }

        #endregion

        private MessageBox _messageboxWarning;
        public MessageBox MessageboxWarning
        {
            get {
                _messageboxWarning = new MessageBox(exportEle, "#32770");
                return _messageboxWarning; }
        }

        #region Select

       /// <summary>
       /// Wizard pane
       /// </summary>
        private Pane _pane;
        public Pane Pane
        {
            get {
                _pane = new Pane(exportEle, "frame_finish");
                return _pane; } 
        }

       /// <summary>
       /// 
       /// </summary>
        private Pane _ReexportPane;
        public Pane ReexportPane
        {
            get {
                _ReexportPane = new Pane(exportEle, "frame_keyslogselect");
                return _ReexportPane; }
          
        }
        /// <summary>
        /// Product key list for Assign Window
        /// </summary>
        private TabControl _tab;
        public TabControl Tab
        {
            get
            {
                _tab = new TabControl(exportEle, "TabControl");
                return _tab;
            }
        }

       /// <summary>
       /// 
       /// </summary>
        private DataGrid _dataGridReExport;
        public DataGrid DataGridReExport
        {
            get {
                _dataGridReExport = new DataGrid(ReexportPane.PaneEle, "DataGrid_1");
                return _dataGridReExport; }
        }

        private RadioButton _RdbtnReExport;
        public RadioButton RdbtnReExport
        {
            get {
                _RdbtnReExport =  new RadioButton(DataGridReExport.DatagridElement);
                return _RdbtnReExport; }
         
        }

        /// <summary>
        /// DataGrid by Quantity
        /// </summary>
        private CommDataGrid _dataGridByQuantity;
        public CommDataGrid DataGridByQuantity
        {
            get
            {
                _dataGridByQuantity = new CommDataGrid(exportEle, "dgByQuantity");
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
                _dataGridByid = new CommDataGrid(exportEle, "dgByKeys");
                return _dataGridByid;
            }
        }

       /// <summary>
       /// DataGrid in the Summary panel
       /// </summary>
        private DataGrid _dataGridSummary;
        public DataGrid DataGridSummary
        {
            get {
                _dataGridSummary = new DataGrid(Pane.PaneEle, "dgKeys");
                return _dataGridSummary;
            }
        }

       /// <summary>
       /// File Location in the Summary panel
       /// </summary>
        private TextBox _textBoxFileName;
        public TextBox TextBoxFileName
        {
            get
            {
                _textBoxFileName = new TextBox(exportEle, "txtFileName");
                return _textBoxFileName; }
        }

        /// <summary>
        /// Target Inventory
        /// </summary>
        private ComboBox _comboBox;
        public ComboBox ComboBox
        {
            get
            {
                _comboBox = new ComboBox(exportEle, "drpSubsidiary");
                return _comboBox;
            }
        }

        #endregion

       /// <summary>
       /// Encrypted Check box
       /// </summary>
        private CheckBox _encrypted;
        public CheckBox Encrypted
        {
            get {
                _encrypted = new CheckBox(exportEle, "chkEncrypt");
                return _encrypted; }
   
        }


       /// <summary>
       /// Path textBox
       /// </summary>
        private TextBox _path;
        public TextBox Path
        {
            get
            {
                _path = new TextBox(exportEle, "txtFile");
                return _path;
            }
        }

        #region Button list
        /// <summary>
       /// Next button
       /// </summary>
        private Button _nextButton;
        public Button NextButton
        {
            get {
                _nextButton = new Button(exportEle, "btnNext");
                return _nextButton; }
        }

        private Button _viewButton;
        public Button ViewButton
        {
            get
            {
                _viewButton = new Button(exportEle, "btnView"); 
                return _viewButton; }
        
        }


        /// <summary>
        /// OK button
        /// </summary>
        private Button _OKbutton;
        public Button OKButton
        {
            get
            {
                _OKbutton = new Button(exportEle, "btnOK");
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
                _cancelbutton = new Button(exportEle, "btnCancel");
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
                _closebutton = new Button(exportEle, "btnFinish");
                return _closebutton;
            }
        }
        #endregion
        private ExportView _exportView;

        public ExportView ExportView
        {
            get {
                _exportView = new ExportView(exportEle, "ViewFileKey");
                return _exportView; }
        }
    }

   public class ExportView : BaseWindow
   {
       private AutomationElement ViewexportEle;
       public ExportView(AutomationElement parentEle, string title)
            : base(parentEle, title)
        {
            ViewexportEle = MainElement;
        }

       private Button _CloseBtn;

       public Button CloseBtn
       {
           get {
               _CloseBtn = new Button(ViewexportEle, "btnClose");
               return _CloseBtn; }
          
       }
       /// <summary>
       /// 
       /// </summary>
       private DataGrid _dataGridViewFile;
       public DataGrid DataGridViewFile
       {
           get {
               _dataGridViewFile = new DataGrid(ViewexportEle, "tbcExportKeys");
               return _dataGridViewFile; }
           
       }
   }
}

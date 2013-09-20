using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
   public class RevertKeyWindow: CommonWindow
    {
        private AutomationElement revertEle;
        public RevertKeyWindow(AutomationElement parent, string name)
            : base(parent, name)
        {
            revertEle = MainElement;
        }

       /// <summary>
       /// 
       /// </summary>
        private Pane _pane;
        public Pane Pane
        {
            get {
                _pane = new Pane(revertEle, "frame_operateMsg");
                return _pane; }
          
        }

       /// <summary>
       /// 
       /// </summary>
        private Pane _paneSummary;
        public Pane PaneSummary
        {
            get {
                _paneSummary = new Pane(revertEle, "frameList");
                return _paneSummary; }
         
        }

       /// <summary>
       /// 
       /// </summary>
        private DataGrid _dataGridOperate;
        public DataGrid DataGridOperate
        {
            get {
                _dataGridOperate = new DataGrid(Pane.PaneEle, "dgKeys");
                return _dataGridOperate; }
   
        }

       /// <summary>
       /// DataGrid of Summary pane
       /// </summary>
        private DataGrid _dataGridSummary;
        public DataGrid DataGridSummary
        {
            get {
                _dataGridSummary = new DataGrid(PaneSummary.PaneEle, "dgKeys");
                return _dataGridSummary; }
          
        }

        /// <summary>
        /// Key list
        /// </summary>
        private CommDataGrid _dataGrid;
        public CommDataGrid DataGrid
        {
            get
            {
                _dataGrid = new CommDataGrid(revertEle, "dgKeys");
                return _dataGrid;
            }
        }

       /// <summary>
       /// The reason of Revert
       /// </summary>
        private MessageBox _msgWaring;
        public MessageBox MsgWaring
        {
            get {
                _msgWaring = new MessageBox(revertEle, "#32770");
                return _msgWaring; }
          
        }

       /// <summary>
       /// 
       /// </summary>
        private TextBox _txtBoxReason;
        public TextBox TxtBoxReason
        {
            get {
                _txtBoxReason = new TextBox(revertEle, "txtReason");
                return _txtBoxReason; }
      
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
                _OKbutton = new Button(revertEle, "btnOK");
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
                _cancelbutton = new Button(revertEle, "btnCancel");
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
                _closebutton = new Button(revertEle, "btnFinish");
                return _closebutton;
            }
        }
        #endregion
    }
}

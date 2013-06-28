using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
   public class UnassignKeyWizard : CommonWindow
    {
       private AutomationElement unassignEle;
       public UnassignKeyWizard(AutomationElement parent, string name)
           : base(parent, name)
       {
           unassignEle = MainElement;
       }

       /// <summary>
       /// Unassign from
       /// </summary>
       private ComboBox _taget;
       public ComboBox Taget
       {
           get {
               _taget = new ComboBox(unassignEle, "drpTpi");
               return _taget; }
       }

       /// <summary>
       /// Key list in the Selec pane.
       /// </summary>
       private CommDataGrid _datagrid;

       public CommDataGrid Datagrid
       {
           get {
               _datagrid = new CommDataGrid(unassignEle, "DataGrid_1");
               return _datagrid; }
       }

       /// <summary>
       /// Summary Pane.
       /// </summary>
       private Pane _pane;
       public Pane Pane
       {
           get {
               _pane = new Pane(unassignEle, "frame_summary");
               return _pane; }
       }

       /// <summary>
       /// Key list in the Summary pane.
       /// </summary>
       private DataGrid _dataGridSummary;
       public DataGrid DataGridSummary
       {
           get {
               _dataGridSummary = new DataGrid(Pane.PaneEle, "DataGrid_1");
               return _dataGridSummary; }
       }

       #region All Button in the Wizard
       /// <summary>
       /// Previous button
       /// </summary>
       private Button _previousbutton;
       public Button PreviousButton
       {
           get
           {
               _previousbutton = new Button(unassignEle, "btnPrevious");
               return _previousbutton;
           }
       }

       /// <summary>
       /// Next button
       /// </summary>
       private Button _nextbutton;
       public Button NextButton
       {
           get
           {
               _nextbutton = new Button(unassignEle, "btnNext");
               return _nextbutton;
           }
       }

       /// <summary>
       /// OK button for Unassign
       /// </summary>
       private Button _OKbutton;
       public Button OKButton
       {
           get
           {
               _OKbutton = new Button(unassignEle, "btnUnAssign");
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
               _cancelbutton = new Button(unassignEle, "btnCancel");
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
               _closebutton = new Button(unassignEle, "btnFinish");
               return _closebutton;
           }
       }

       #endregion
    }
}

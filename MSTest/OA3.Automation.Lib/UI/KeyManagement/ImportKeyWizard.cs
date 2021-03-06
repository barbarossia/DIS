﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
   public  class ImportKeyWizard: BaseWindow
    {
       private AutomationElement importElement;
       public ImportKeyWizard(AutomationElement ParentEle, string Name)
           : base(ParentEle, Name)
       {
           importElement = MainElement;
       }

       #region Select Type
       /// <summary>
       /// Import Keys or CBR
       /// </summary>
       private RadioButton _importKeysCBR;
       public RadioButton ImportKeysCBR
       {
           get
           {
               _importKeysCBR = new RadioButton(importElement, "Rbkeys");
               return _importKeysCBR;
           }
       }

       /// <summary>
       /// Import Duplicate CBR
       /// </summary>
       private RadioButton _importDupCBR;
       public RadioButton ImportDupCBR
       {
           get
           {
               _importDupCBR = new RadioButton(importElement, "RbDuplicateCBR");
               return _importDupCBR;
           }
       }

       /// <summary>
       /// Import file generated by OA3 tool
       /// </summary>
       private RadioButton _importOAtool;
       public RadioButton ImportOAtool
       {
           get
           {
               _importOAtool = new RadioButton(importElement, "RbToolkeys");
               return _importOAtool;
           }
       }


       /// <summary>
       /// Next button
       /// </summary>
       private Button _nextButton;
       public Button NextButton
       {
           get {
               _nextButton = new Button(importElement, "btnNext");
               return _nextButton; }
      
       }


       /// <summary>
       /// Location 
       /// </summary>
       private TextBox _pathTextbox;
       public TextBox PathTextbox
       {
           get
           {
               _pathTextbox = new TextBox(importElement, "TextBox_1");
               return _pathTextbox; }
       }

       /// <summary>
       /// DataGrid of the Summary pane.
       /// </summary>
       private DataGrid _dataGridSummary;
       public DataGrid DataGridSummary
       {
           get {
               _dataGridSummary = new DataGrid(importElement, "DataGrid_1");
               return _dataGridSummary; }
       }


       /// <summary>
       /// Pane of Result window
       /// </summary>
       private Pane _paneResult;
       public Pane PaneResult
       {
           get {
               _paneResult = new Pane(importElement, "frame_importresult");
               return _paneResult; }
         
       }
       /// <summary>
       /// the number of Total keys 
       /// </summary>
       private Label _labelTotalKeys;
       public Label LabelTotalKeys
       {
           get {
               _labelTotalKeys = new Label(PaneResult.PaneEle, "Label_3");
               return _labelTotalKeys; }
       }

       #endregion

       #region Button list
       /// <summary>
       /// OK button
       /// </summary>
       private TempButton _OKButton;
       public TempButton OKButton
       {
           get
           {
               _OKButton = new TempButton(importElement, "Button_1");
               return _OKButton;
           }

       }


       /// <summary>
       /// Previous button
       /// </summary>
       private Button _previousButton;
       public Button PreviousButton
       {
           get {
               _previousButton = new Button(importElement, "btnPrevious");
               return _previousButton; }
       }

       /// <summary>
       /// Cancel button
       /// </summary>
       private Button _closeButton;
       public Button CloseButton
       {
           get
           {
               _closeButton = new Button(importElement, "btnFinish");
               return _closeButton;
           }
       }


       #endregion

       /// <summary>
       /// Error messagebox
       /// </summary>
       private MessageBox _messageBoxError;
       public MessageBox MessageBoxError
       {
           get {
               _messageBoxError = new MessageBox(importElement, "#32770");
               return _messageBoxError; }
       }

       /// <summary>
       /// 
       /// </summary>
       private ProgressBar _progressBar;
       public ProgressBar ProgressBar
       {
           get
           {
               _progressBar = new ProgressBar(importElement, "ProgressIndicator");
               return _progressBar;
           }

       }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WPFAutomation.Core;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
    public class DISRibbon : Ribbon
    { 
        private AutomationElement _ribbon;
        public DISRibbon(AutomationElement Ele, string className): base(Ele,className)
        {
            _ribbon = Helper.ExtractElementByClassName(Ele, "Ribbon");
        }


        #region Key mangement
        /// <summary>
        /// Export Buton
        /// </summary>
        private Button _exportButton;
        public Button ExportButton
        {
            get
            {
                _exportButton = new Button(_ribbon, "rbnbExport");
                return _exportButton;
            }
        }


        /// <summary>
        /// Import Button
        /// </summary>
        private Button _importButton;
        public Button ImportButton
        {
            get
            {
                _importButton = new Button(_ribbon, "btnimport");
                return _importButton;
            }
        }

        /// <summary>
        /// Get Keys button
        /// </summary>
        private Button _getKeys;
        public Button GetKeys
        {
            get
            {
                _getKeys = new Button(_ribbon, "rbnbGetKeys");
                return _getKeys;
            }

        }

        /// <summary>
        /// Report Keys button
        /// </summary>
        private Button _reportKeys;
        public Button ReportKeys
        {
            get
            {
                _reportKeys = new Button(_ribbon, "rbnbReportKeys");
                return _reportKeys;
            }
        }

        /// <summary>
        /// Assign Keys button
        /// </summary>
        private Button _assignKeys;
        public Button AssignKeys
        {
            get
            {
                _assignKeys = new Button(_ribbon, "rbnbAssign");
                return _assignKeys;
            }

        }

        /// <summary>
        /// UnAssign Keys button
        /// </summary>
        private Button _unassignKeys;
        public Button UnassignKeys
        {
            get
            {
                _unassignKeys = new Button(_ribbon, "btnunmark");
                return _unassignKeys;
            }

        }

        /// <summary>
        /// UnAssign Keys button
        /// </summary>
        private Button _refreshButton;
        public Button RefreshButton
        {
            get
            {
                _refreshButton = new Button(_ribbon, "btnrefreshkey");
                return _refreshButton;
            }

        }

        /// <summary>
        /// Recall Keys button
        /// </summary>
        private Button _recallButton;
        public Button RecallButton
        {
            get
            {
                _recallButton = new Button(_ribbon, "rbnbRecallKeys");
                return _recallButton;
            }

        }


        /// <summary>
        /// Revert Keys button
        /// </summary>
        private Button _revertButton;
        public Button RevertButton
        {
            get
            {
                _revertButton = new Button(_ribbon, "rbnbRevertKeys");
                return _revertButton;
            }

        }

        #endregion

        #region User Management
        /// <summary>
        /// User management- add user button
        /// </summary>
        private Button _adduserbutton;

        public Button Adduserbutton
        {
            get {

                _adduserbutton = new Button(_ribbon, "btnadduser");
                return _adduserbutton;

            }
           
        }
        /// <summary>
        /// User management- edit user button
        /// </summary>
        private Button _edituserbutton;

        public Button Edituserbutton
        {
            get {
                _edituserbutton = new Button(_ribbon, "btnedituser");
                return _edituserbutton;

            }
        
        }
        /// <summary>
        /// User management- remove user button
        /// </summary>
        private Button _removeuserbutton;

        public Button Removeuserbutton
        {
            get {
                _removeuserbutton = new Button(_ribbon ,"btnremoveuser");
                return _removeuserbutton;
            
            
            
            }
            
        }
        /// <summary>
        /// User management- search button
        /// </summary>
        private Button _searchbutton;

        public Button Searchbutton
        {
            get {
                _searchbutton = new Button(_ribbon, "btnfilter");
                return _searchbutton;
            
            
            }
            
        }

        /// <summary>
        /// User management - role combobox button
        /// </summary>
        private ComboBox _rolecombox;

        public ComboBox Rolecombox
        {
            get {
                _rolecombox = new ComboBox(_ribbon, "ComboBox_1");
                return _rolecombox;
            }
         
        }

        #endregion 

        #region Log Viewer
        /// <summary>
        /// log viewer - log export button
        /// </summary>

        private Button _exportbutton;

        public Button Exportbutton
        {
            get {

                _exportbutton = new Button(_ribbon, "btnexportlog");

                return _exportbutton;
            
            }
         
        }
        /// <summary>
        /// log viewer - log type combobox
        /// </summary>
        private ComboBox _logtypecombobox;

        public ComboBox Logtypecombobox
        {
            get {

                _logtypecombobox = new ComboBox(_ribbon, "ComboBox_2");
                return _logtypecombobox;
            
            }
        
        }
        /// <summary>
        /// log viewer - refresh button
        /// </summary>
        private Button _refreshbutton;

        public Button Refreshbutton
        {
            get {


                _refreshbutton = new Button(_ribbon, "btnrefreshlog");
                return _refreshbutton;
            
            
            }

        }

        #endregion


    }
}

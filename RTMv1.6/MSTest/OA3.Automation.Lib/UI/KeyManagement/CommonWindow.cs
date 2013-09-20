using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core;
using System.Windows.Automation;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI.Windows
{
    public class CommonWindow :BaseWindow
    {
        private AutomationElement Element;
        public CommonWindow(AutomationElement CommElement, string name)
            : base(CommElement, name)
        {
            Element = MainElement;
        }

        #region Search Condition
        /// <summary>
        /// MS fulfilled Data from
        /// </summary>
        private DatePicker _startDataPicker;
        public DatePicker StartDataPicker
        {
            get
            {
                _startDataPicker = new DatePicker(Element, "StartChangeStateDate");
                return _startDataPicker;
            }
        }

        /// <summary>
        /// MS fulfilled data to
        /// </summary>
        private DatePicker _endDataPicker;
        public DatePicker EndDataPicker
        {
            get
            {
                _endDataPicker = new DatePicker(Element, "EndChangeStateDate");
                return _endDataPicker;
            }
        }


        /// <summary>
        /// Search condition of OEM PO Number
        /// </summary>
        private TextBox _OEMPOnumber;
        public TextBox OEMPOnumber
        {
            get
            {
                _OEMPOnumber = new TextBox(Element, "SearchPONumber");
                return _OEMPOnumber;
            }
        }


        /// <summary>
        /// Search condition of OEM Part Number
        /// </summary>
        private TextBox _OEMPartnumber;
        public TextBox OEMPartnumber
        {
            get
            {
                _OEMPartnumber = new TextBox(Element, "_6");
                return _OEMPartnumber;
            }
        }

        /// <summary>
        /// Search condition of MS Part Number
        /// </summary>
        private TextBox _MsPartnumber;
        public TextBox MsPartnumber
        {
            get
            {
                _MsPartnumber = new TextBox(Element, "_5");
                return _MsPartnumber;
            }
        }

        /// <summary>
        /// Search Button
        /// </summary>
        private Button _searchButton;
        public Button SearchButton
        {
            get
            {
                _searchButton = new Button(Element, "Button_1");
                return _searchButton; }
                 }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private ProgressBar _progressBar;
        public ProgressBar ProgressBar
        {
            get
            {
                _progressBar = new ProgressBar(Element, "ProgressIndicator");
                return _progressBar;
            }

        }
    }
}

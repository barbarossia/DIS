using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core.Controls;
using System.Windows.Automation;
using OA3.Automation.Lib.Log;

namespace OA3.Automation.Lib.UI.Windows
{
   public class CommDataGrid : DataGrid
    {
        public CommDataGrid(AutomationElement parent, string name)
            : base(parent, name)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="quantity"></param>
        public void SetValue(int index, string value)
        {
            TextBox textBox = new TextBox(DatagridElement, ControlType.Edit);
            textBox.SetValue(index, value);
            index++;
            TextLog.LogMessage("Set value to ["+index+"] item in the DataGrid by quantity.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void CheckItem(int index)
        {
            CheckBox checkbox = new CheckBox(DatagridElement, ControlType.CheckBox);
            checkbox.Click(index);
            TextLog.LogMessage("Check ["+index+"] item in the DataGrid by Keys.");
        }
    }
}

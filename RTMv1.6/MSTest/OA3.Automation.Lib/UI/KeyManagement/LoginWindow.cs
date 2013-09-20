using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core;
using System.Windows.Automation;
using OA3.Automation.Lib.Log;
using WPFAutomation.Core.Controls;
using OA3.Automation.Lib.KMT;
using System.Threading;
using System.Reflection;

namespace OA3.Automation.Lib.UI.Windows
{
    public class LoginWindow : BaseWindow
    {
        public string appPath = @"C:\Program Files (x86)\DIS Solution\TPI\Key Management Tool\DIS.Presentation.KMT.exe";
      
        private AutomationElement TempEle;
        public LoginWindow():base()
        {
            TempEle = Driver.StartApplication(appPath);
            if (TempEle != null)
            {
                TextLog.LogMessage("Login Window has been successfully loaded."); 
            }
        }

        public LoginWindow(AutomationElement mainElement, string title)
            : base(mainElement, title)
        {
            TempEle = MainElement;
            if (TempEle != null)
            {
                TextLog.LogMessage("Login Window has been successfully loaded.");
            }
        }

        /// <summary>
        /// User Login ID
        /// </summary>
        private TextBox _UsernameTextBox;
        public TextBox UserNameTxtBox
        {
            get
            {
                this._UsernameTextBox = new TextBox(TempEle, "txtUserName");
                return _UsernameTextBox;
            }
        }

        /// <summary>
        /// User Password
        /// </summary>
        private TextBox _passwordTextbox;
        public TextBox PasswordTxtBox
        {
            get
            {
                this._passwordTextbox = new TextBox(TempEle, "PasswordBox_1");
                return _passwordTextbox;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Button _loginButton;
        public Button LoginButton
        {
            get
            {
                _loginButton = new Button(TempEle, "btnlogin");
                return _loginButton;
            }
        }

        private Button _canelButton;
        public Button CanelButton
        {
            get
            {
                _canelButton = new Button(TempEle, "btnCancel");
                return _canelButton;
            }
        }

        private MessageBox _messagebox;
        public MessageBox messagebox
        {
            get
            {
                _messagebox = new MessageBox(TempEle, "#32770");
                return _messagebox;
            }
        }

        private Label _lable;
        public Label lable
        {
            get
            {
                _lable = new Label(TempEle, "65535");
                return _lable;
            }
        }

        public void closeApp()
        {
            WPFAutomation.Core.Helper.CloseApp(TempEle);
        }

        public AutomationElement LoginSucc()
        {
           
            Thread.Sleep(1000);
            AutomationElement element = null;
            //for (int i = 0; i < ReaderXML.loginData.Count; i++)
            //{
            //    UserNameTxtBox.SetValue(ReaderXML.loginData["loginSucc_" + i].UserName);
            //    PasswordTxtBox.SetValue(ReaderXML.loginData["loginSucc_" + i].PassWord);
            //    LoginButton.Click();
            //    Thread.Sleep(10000);

            //    element = Driver.FindApplicationByTitle(ReaderXML.loginData["loginSucc_" + i].Message);
            //    bool reasult = element == null ? false : true;
            //    Verification.AssertKMTResponse(reasult, MethodBase.GetCurrentMethod().Name + "_Success_" + i);
            //}

            //if (element == null)
            //{
            //    Console.WriteLine("Loading KMT is fail!");
            //}
            return element;
        }
       
    }
}

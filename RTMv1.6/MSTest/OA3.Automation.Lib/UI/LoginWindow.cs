using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFAutomation.Core;
using System.Windows.Automation;
using WPFAutomation.Core.Controls;

namespace OA3.Automation.Lib.UI
{
   public class LoginWindow : BaseWindow
    {
       private AutomationElement LoginWindowEle;
       public LoginWindow(AutomationElement parentEle, string title)
           : base(parentEle, title)
       {
           LoginWindowEle = MainElement;
       }

       /// <summary>
       /// TextBox of Login ID
       /// </summary>
       private TextBox _textBoxLoginID;
       public TextBox TextBoxLoginID
       {
           get {
               _textBoxLoginID = new TextBox(LoginWindowEle, "txtUserName");
               return _textBoxLoginID; }
       
       }


       /// <summary>
       /// TextBox of Password
       /// </summary>
       private TextBox _textBoxPassword;
       public TextBox TextBoxPassword
       {
           get {
               _textBoxPassword = new TextBox(LoginWindowEle, "pwdPwd");
               return _textBoxPassword; }
     
       }

       /// <summary>
       /// Login Button
       /// </summary>
       private Button _BtnLogin;
       public Button BtnLogin
       {
           get {
               _BtnLogin = new Button(LoginWindowEle, "btnLogin");
               return _BtnLogin; }
         
       }

       /// <summary>
       /// Canel Button
       /// </summary>
       private Button _canelButton;
       public Button CanelButton
       {
           get {
               _canelButton = new Button(LoginWindowEle, "btnCancel");
               return _canelButton; }
          
       }

       /// <summary>
       /// Error message
       /// </summary>
       private MessageBox _msgBoxError;
       public MessageBox MsgBoxError
       {
           get {
               _msgBoxError = new MessageBox(LoginWindowEle, "#32770");
               return _msgBoxError; }   
       }

       private ProgressBar _progressBar;
       public ProgressBar ProgressBar
       {
           get {
               _progressBar = new ProgressBar(LoginWindowEle, "progressBar");
               return _progressBar; }
         
       }
    }
}

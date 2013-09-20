using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OA3.Automation.Lib.KMT
{
    public class Userinfo
    {
        /// <summary>
        /// login id for User management
        /// </summary>
        private string _loginID;

        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }
        /// <summary>
        /// password for User management
        /// </summary>
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        /// <summary>
        /// confirmpassword for User management
        /// </summary>
        private string _confirmpassword;

        public string Confirmpassword
        {
            get { return _confirmpassword; }
            set { _confirmpassword = value; }
        }
        /// <summary>
        /// firstname for User management
        /// </summary>
        private string _firstname;

        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        /// <summary>
        /// lastname for User management
        /// </summary>
        private string _lastname;

        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        /// <summary>
        /// department for User management
        /// </summary>
        private string _department;

        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }
        /// <summary>
        /// position for User management
        /// </summary>
        private string _position;

        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
        /// <summary>
        /// phone for User management
        /// </summary>
        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        /// <summary>
        /// email for User management
        /// </summary>
        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// role for User management
        /// </summary>
        private string _role;

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public Userinfo()
        { 
        
        
        
        }
        public Userinfo(string loginid ,string password,string confirmpassword,string role)
        {
            _loginID = loginid;
            _Password = password;
            _confirmpassword = confirmpassword;
            _role = role;
  
        }




    }
}

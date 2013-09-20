using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OA3.Automation.Lib.Log;

namespace OA3.Automation.Lib.KMT
{
    public class TestModule
    {
        public TestModule() { }
       
        /// <summary>
        /// 
        /// </summary>
        private string _assembly;
        public string Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, TestCase> _case = new Dictionary<string, TestCase>();
        public Dictionary<string, TestCase> Case
        {
            get
            {
                return _case;
            }
            set
            {
                _case = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// Detail info of Case
    /// </summary>
    public class TestCase
    {
        //testcase name
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _cat;
        public string Cat
        {
            get { return _cat; }
            set { _cat = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _owner;
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private CaseResult _result;
        public CaseResult Result
        {
            get { return _result; }
            set { _result = value; }
        }

     

        /// <summary>
        /// 
        /// </summary>
        public List<string> FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> MethodName { get; set; }

        /// <summary>
        /// Action List
        /// </summary>
        public List<string> actionList { get; set; }

        public enum CaseResult { NotRun, Pass, Fail };

        /// <summary>
        /// Input Data
        /// </summary>
        private Dictionary<string, string> _input = new Dictionary<string, string>();
        public Dictionary<string, string> Input
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// Expect result
        /// </summary>
        private Dictionary<string, string> _expected = new Dictionary<string, string>();

        public Dictionary<string, string> Expected
        {
            get { return _expected; }
            set { _expected = value; }
        }

        public override string ToString()
        {
            string s = "Testcase cat: " + Cat + ", id: " + Id + ", owner :" + Owner + ".";
            return s;
        }

        /// <summary>
        /// Run this testcase
        /// Wilson
        /// </summary>
        public void Run_Wilson(Assembly testcasesAssembly)
        {
            //testcase start
            TextLog.LogMessage(this.ToString());
            foreach (string action in actionList)
            {
                int dotIndex = action.LastIndexOf('.');
                string classType = action.Remove(dotIndex);
                string methodName = action.Substring(dotIndex + 1);
                Type type = testcasesAssembly.GetType(classType);
                object instance = Activator.CreateInstance(type, null);

                try
                {
                    type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instance, null);
                }
                catch (Exception e)
                {
                    Log.TextLog.LogMessage(e.Message);
                    Log.TextLog.LogMessage(e.StackTrace);
                }

                //type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instance, null);

                if (this.Result != CaseResult.Pass)
                    break;
            }
            //testcase end
            TextLog.LogMessage("=======================================================");
        }

 }

}

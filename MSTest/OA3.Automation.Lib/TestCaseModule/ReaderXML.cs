using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using OA3.Automation.Lib.Log;
using WPFAutomation.Core;

namespace OA3.Automation.Lib.KMT
{
    public class ReaderXML
    {
        private static XmlDocument doc;
        public static TestModule testModule = new TestModule();
        public static TestModule LoginInfo = new TestModule();
        public static string CurrentCase;
        public ReaderXML()
        {
            // InitializeStaticMembers();
            // TextLog.LogMessage("Initialize Test Data Successfully.");
        }

        public static void CleanData()
        {
            testModule.Case.Clear();
            LoginInfo.Case.Clear();
        }


        /// <summary>
        /// Load Test Data
        /// </summary>
        /// <param name="doc"></param>
        public static void LoadTestData(string fileName)
        {
            LoadData(fileName, testModule);
        }

        public static void LoadLogin(string fileName)
        {

            LoadData(fileName, LoginInfo);
        }

        private static void LoadData(string filePath, TestModule module)
        {
            module.Case.Clear();
            doc = new XmlDocument();
            try
            {
                doc.Load(filePath);
                module.Assembly = ReadSingleNode(doc, "testcases", "assemble");
                XmlNodeList nodeList = doc.SelectNodes("testcases/case");
                XmlNode currentNode;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    TestCase testCase = new TestCase();
                    currentNode = nodeList[i];
                    testCase.Result = TestCase.CaseResult.NotRun;
                    testCase.Id = ReadNodeAttribute(currentNode, "id");
                    testCase.Cat = ReadNodeAttribute(currentNode, "cat");
                    testCase.Owner = ReadNodeAttribute(currentNode, "owner");

                    if (currentNode.HasChildNodes)
                    {
                        XmlNodeList childNodelist = currentNode.ChildNodes;
                        foreach (XmlNode item in childNodelist)
                        {
                            switch (item.Name)
                            {
                                case "inputs": testCase.Input = ReadChildNode(item); break;
                                case "expected": testCase.Expected = ReadChildNode(item); break;
                                case "actionList": testCase.MethodName = ReadChildNode(item, NameType.MethodName);
                                    testCase.FullName = ReadChildNode(item, NameType.FullName);
                                    testCase.actionList = ReadChildNode(item, NameType.Default); break;
                                default:
                                    break;
                            }
                        }
                    }
                    module.Case.Add(testCase.Id, testCase);
                }
                module.Total = module.Case.Count;
            }
            catch (NotSupportedException err)
            {

                TextLog.LogMessage("Loading XML file failed! info: " + err.Message);
            }
        }

        private static Dictionary<string, string> ReadChildNode(XmlNode nodelist)
        {

            Dictionary<string, string> Node = new Dictionary<string, string>();
            if (nodelist.HasChildNodes)
            {
                XmlNodeList inputList = nodelist.ChildNodes;
                foreach (XmlNode item in inputList)
                {
                    Node.Add(ReadNodeAttribute(item, "name"), ReadNodeAttribute(item, "value"));
                }
            }

            return Node;
        }

        private static List<string> ReadChildNode(XmlNode nodelist, NameType type)
        {

            List<string> actionList = new List<string>();
            if (nodelist.HasChildNodes)
            {
                XmlNodeList inputList = nodelist.ChildNodes;
                foreach (XmlNode item in inputList)
                {
                    if (type == NameType.FullName)
                    {
                        actionList.Add(getFullName(item.InnerText.ToString()));
                    }
                    else if (type == NameType.MethodName)
                    {
                        actionList.Add(getMethodName(item.InnerText.ToString()));
                    }
                    else
                    {
                        actionList.Add(item.InnerText.ToString());
                    }
                }
            }


            return actionList;
        }

        private static string ReadNodeAttribute(XmlNode node, string attribute)
        {
            string value = "";
            try
            {
                value =
                    node.Attributes[attribute].Value;
            }
            catch (Exception)
            {
                TextLog.LogMessage("Can not get value of " + attribute);
            }
            return value;
        }

        private static string ReadSingleNode(XmlDocument xmlDoc, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlNode xnode = xmlDoc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xnode.InnerText : xnode.Attributes[attribute].Value);
            }
            catch (Exception)
            {
                TextLog.LogMessage("Can not get value of " + attribute);
            }
            return value;
        }

        /// <summary>
        /// Excute Test Case
        /// </summary>
        /// <param name="currentTestCase"></param>
        public static void ExcuteTestCase(string currentTestCase)
        {
            ReaderXML.CurrentCase = currentTestCase;
            for (int i = 0; i < ReaderXML.testModule.Case[currentTestCase].actionList.Count; i++)
            {
                Driver.ExcuteMethodByName(
                               ReaderXML.testModule.Assembly,
                               ReaderXML.testModule.Case[currentTestCase].FullName[i],
                               ReaderXML.testModule.Case[currentTestCase].MethodName[i],
                                null);
            }


        }

        /// <summary>
        /// Excute All Case
        /// </summary>
        public static void ExcuteAllTestCase()
        {
            foreach (var item in ReaderXML.testModule.Case)
            {
                ExcuteTestCase(item.Key);
            }
        }
        /// <summary>
        /// Get MethodName
        /// </summary>
        /// <param name="invoke"></param>
        /// <returns></returns>
        private static string getMethodName(string invoke)
        {
            try
            {
                return invoke.Substring(invoke.LastIndexOf(".") + 1);
            }
            catch (Exception)
            {
                return invoke;
            }


        }
        /// <summary>
        /// Get Full Name 
        /// </summary>
        /// <param name="invoke"></param>
        /// <returns></returns>
        private static string getFullName(string invoke)
        {
            try
            {
                int endindex = invoke.LastIndexOf(".");
                return invoke.Substring(0, endindex);
            }
            catch (Exception)
            {
                return invoke;
            }
        }

        enum NameType { MethodName, FullName, Default };

        /// <summary>
        /// Run some testcases
        /// Wilson
        /// </summary>
        /// <param name="ids"></param>
        public void RunTestcases_Wilson(string[] ids)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            string assembleFile = Path.GetFullPath(testModule.Assembly);
            if (!File.Exists(assembleFile))
            {
                TextLog.LogMessage("Could not find file (" + assembleFile + ").");
                TextLog.LogMessage("Try to find it in current directory...");
                assembleFile = Environment.CurrentDirectory + "\\" + Path.GetFileName(assembleFile);
                if (!File.Exists(assembleFile))
                {
                    TextLog.LogMessage("Could not find file (" + assembleFile + ").");
                    return;
                }
            }
            Assembly testcasesAssembly = Assembly.LoadFile(assembleFile);
            foreach (string id in ids)
            {
                if (testModule.Case.ContainsKey(id))
                {
                    CurrentCase = id;
                    testModule.Case[id].Run_Wilson(testcasesAssembly);
                }
            }
            this.Summarize(ids);
        }

        /// <summary>
        /// Run all testcases
        /// Wilson
        /// </summary>
        public void RunTestcases_Wilson()
        {
            this.RunTestcases_Wilson(testModule.Case.Keys.ToArray());
        }

        /// <summary>
        /// summary of some testcases
        /// </summary>
        /// <param name="ids"></param>
        private void Summarize(string[] ids)
        {
            int fail = 0, pass = 0;
            foreach (string id in ids)
            {
                if (testModule.Case.ContainsKey(id))
                {
                    switch (testModule.Case[id].Result)
                    {
                        case TestCase.CaseResult.Pass:
                            pass++;
                            break;
                        case TestCase.CaseResult.Fail:
                            fail++;
                            break;
                    }
                }
            }
            TextLog.LogMessage("Total: " + testModule.Case.Count + ", selected: " + ids.Length + ".");
            TextLog.LogMessage("Pass: " + pass + ", fail: " + fail + ", unkown: " + (ids.Length - pass - fail) + ".");
        }

    }
}


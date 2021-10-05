using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using RestSharp;
using TechTalk.SpecFlow;
using UNITE.Utils;

/*
 * Created By: Chirag
*/

namespace UNITE.Core
{
    [Binding]
    public class BDDdriver
    {
        //Global Variable for Extend report
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        private static RestClient restClient;

        APIDriver APIDriver = new BaseDriver();
        static List<object> logs;

        public static void AddLog(object log = null)
        {
            Console.WriteLine(log);
            logs.Add(log);
        }

        public void SetAPIDriver()
        {
            restClient = APIDriver.apiinit();
        }

        public RestClient GetAPIDriver()
        {
            return restClient;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            BDDdriver obj = new BDDdriver();
            obj.SetAPIDriver();
            //Initialize Extent report before test starts
            SystemPathUtils sysobj = new SystemPathUtils();
           // var htmlReporter = new ExtentHtmlReporter(sysobj.getProjectPath() + @"\Report\ExtentReport.html");
            var htmlReporter = new ExtentHtmlReporter(sysobj.getProjectPath() + @"\Report\" + "ExtentReport_" +  DateTime.Now.ToString("MM_dd_HH_mm") + ".html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.DocumentTitle = "TransUnion APITest Report";            
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            extent.Flush();
        }

        [BeforeFeature]
        [Obsolete]
        public static void BeforeFeature()
        {
            //Create dynamic feature name
            //featureName = extent.CreateTest(FeatureContext.Current.FeatureInfo.Title);
            featureName = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);
        }

        [BeforeStep]
        public void CleanLogs()
        {
            logs = new List<object>();
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext scenarioContext)
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            ExtentTest currentNode = null;
            var currentStepInfo = ScenarioStepContext.Current.StepInfo;

            if (stepType == "Given")
                currentNode = scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "When")
                currentNode = scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "Then")
                currentNode = scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
            else if (stepType == "And")
                currentNode = scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);

            if (currentStepInfo.Table != null)
            {
                currentNode.Info("<table border=\"1px solid black;\" style=\"width: 100%\" cellspacing=\"5px\" cellpadding=\"5px\">" +
                                    "<tbody>");
                currentNode.Info($"<tr>" +
                                     $"<td align=\"Center\" colSpan={currentStepInfo.Table.Header.Count}>--- table step argument ---</td>" +
                                  $"</tr>");
                currentNode.Info("<tr>");
                foreach (var header in currentStepInfo.Table.Header)
                    currentNode.Info($"<th padding=\"3px 5px\">{header}</th>");

                currentNode.Info("</tr>");
                foreach (var row in currentStepInfo.Table.Rows)
                {
                    currentNode.Info("<tr>");
                    foreach (var col in row.Values)
                    {
                        currentNode.Info($"<td padding=\"3px 5px\">{col}</td>");
                    }
                    currentNode.Info("</tr>");
                }
                currentNode.Info("</tbody></table>");
            }

            string methodInfo = currentStepInfo.BindingMatch.StepBinding.Method.Type.Name.Split('.').Last() + "." + currentStepInfo.BindingMatch.StepBinding.Method.Name;
            logs.Insert(0, $"-> done : {methodInfo}");
            if (logs.Count > 0)
            {
                string finalLogs = string.Empty;
                foreach (string log in logs)
                    finalLogs = finalLogs + log + "&#13;&#10;";
                currentNode.Info($"<table border =\"1px solid black;\" style=\"width: 100%\">" +
                                        $"<tbody>" +
                                            $"<tr>" +
                                                $"<td style =\"color : Yellow\" align=\"Center\">LOGS:</td>" +
                                            $"</tr>" +
                                            $"<tr>" +
                                                $"<td><textarea readonly style=\"background-color: gray\">{finalLogs}</textarea></td>" +
                                            $"</tr>" +
                                        $"</tbody>" +
                                    $"</table>");
            }

            if (scenarioContext.TestError != null)
                currentNode.Fail($"<table border =\"1px solid black;\" style=\"width: 100%\">" +
                                        $"<tbody>" +
                                            $"<tr>" +
                                                $"<td style =\"color : Red\" align=\"Center\">ERROR:</td>" +
                                            $"</tr>" +
                                            $"<tr>" +
                                                $"<td><textarea readonly style=\"background-color: gray\">Error Message : {scenarioContext.TestError.Message}&#13;&#10;" +
                                                $"Inner Exception : {scenarioContext.TestError.InnerException}&#13;&#10;" +
                                                $"Stack Trace:&#13;&#10;{scenarioContext.TestError.StackTrace}</textarea></td>" +
                                            $"</tr>" +
                                        $"</tbody>" +
                                    $"</table>");
        }

        [BeforeScenario]
        [Obsolete]
        public void Initialize()
        {
            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void CleanUp()
        {

        }
    }
}
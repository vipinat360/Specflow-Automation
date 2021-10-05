using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using UNITE.Core;

namespace UNITE.StepDefination
{
    [Binding]
    public class HerokuappAPIDEMOSteps
    {
        BDDdriver DriverInstance;
        string url = string.Empty;
        string uri = string.Empty;
        string token = string.Empty;
        IAPIHelper APIDriver;
        static Dictionary<string, object> storedVariables = new Dictionary<string, object>();

        public HerokuappAPIDEMOSteps(BDDdriver bdd)
        {
            this.DriverInstance = bdd;
            APIDriver = new BaseDriverHelper(DriverInstance.GetAPIDriver());
            url = APIDriver.GetBaseURI("url");
        }

        [Given(@"I login to Herokuapp application to get Token using ""(.*)"" and ""(.*)""")]
        public void GivenILoginToHerokuappApplicationToGetTokenUsingAnd(string username, string password)
        {
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"username\" : \"" + username + "\",\"password\" : \"" + password + "\"}");
            APIDriver.SubmitRequest(Method.POST, url + "/auth");
            token = APIDriver.GetSingleValueFromResponse("token");
            if (!storedVariables.ContainsKey("token"))
                storedVariables.Add("token", token);
            APIDriver.Authentication(AuthType.BEARER, token);
        }

        [When(@"I add following Header in the request")]
        public void WhenIAddFollowingHeaderInTheRequest(Table table)
        {
            foreach (var row in table.Rows)
            {
                if (row[1].Contains("{") && row[1].Contains("}"))
                {
                    var value = GetValueBetweenBrackets(row[1]);
                    value = row[1].Replace("{"+value+"}", storedVariables[value].ToString());
                    APIDriver.UpdateRequestHeader(row[0], value);
                }
                else
                    APIDriver.UpdateRequestHeader(row[0], row[1]);
            }
        }

        [When(@"I add following parameter in the request for endpoint ""(.*)""")]
        public void WhenIAddFollowingParameterInTheRequestForEndpoint(string endPoint, Table table)
        {
            Dictionary<string, string> parameterMap = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                parameterMap.Add(row[0], row[1]);
            }
            url = APIDriver.AppendUriWithParameters(url + endPoint, parameterMap);
        }

        [When(@"I submit ""(.*)"" request to the ""(.*)"" endpoint")]
        public void WhenISubmitRequestToTheEndPoint(string method, string endPoint = "")
        {
            if (!string.IsNullOrEmpty(endPoint.Trim()))
                endPoint = url + endPoint;
            else
                endPoint = url;
            if (endPoint.Contains("{"))
            {
                var variable = GetValueBetweenBrackets(endPoint);
                endPoint = endPoint.Replace("{"+variable+"}", storedVariables[variable].ToString());
                //APIDriver.AddUrlSegment(variable, storedVariables[variable]);
            }
            switch (method.ToLower())
            {
                case "get":
                    APIDriver.SubmitRequest(Method.GET, endPoint);
                    break;
                case "post":
                    APIDriver.SubmitRequest(Method.POST, endPoint);
                    break;
                case "put":
                    APIDriver.SubmitRequest(Method.PUT, endPoint);
                    break;
                case "patch":
                    APIDriver.SubmitRequest(Method.PATCH, endPoint);
                    break;
                case "delete":
                    APIDriver.SubmitRequest(Method.DELETE, endPoint);
                    break;
                default:
                    break;
            }
        }

        private string GetValueBetweenBrackets(string value)
        {
            return Regex.Match(value, @"\{(.*?)\}").Groups[1].Value;
        }

        [Then(@"I verify attribute ""(.*)"" has ""(.*)"" value in response")]
        public void ThenIVerifyAttributeHasValueInResponse(string attribute, string value)
        {
            APIDriver.AssertResponseBodyAttribute(attribute, value);
        }


        [Then(@"I should see ""(.*)"" as status code")]
        public void ThenIShouldSeeAsStatusCode(int statusCode)
        {
            APIDriver.AssertStatusCode(statusCode);
        }

        [Then(@"I should see ""(.*)"" as status line")]
        public void ThenIShouldSeeAsStatusLine(string statusLine)
        {
            APIDriver.AssertStatusLine(statusLine);
        }

        [When(@"I add following body in request")]
        public void WhenIAddFollowingBodyInRequest(Table table)
        {
            switch (table.Rows[0][0].ToLower())
            {
                case "json":
                    APIDriver.AddBodyInRequest(RequestBodyType.JSON, table.Rows[0][1]);
                    break;
            }
        }

        [Then(@"I store ""(.*)"" attribute value in ""(.*)"" variable for future use")]
        public void ThenIStoreAttributeValueInVariableForFutureUse(string attribute, string variable)
        {
            if (!storedVariables.ContainsKey(variable))
                storedVariables.Add(variable, APIDriver.GetSingleValueFromResponse(attribute));
        }

        [Then(@"I verify ""(.*)"" is ""(.*)"" available in the response")]
        public void ThenIVerifyIsAvailableInTheResponse(string attribute, string expectedStr = "")
        {
            bool expected = true;
            if (expectedStr.ToLower().Contains("not"))
                expected = false;
            APIDriver.AssertNodeIsPresent(attribute, expected);
        }
    }
}
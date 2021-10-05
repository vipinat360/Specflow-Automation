using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TechTalk.SpecFlow;
using UNITE.Core;
using UNITE.Utils;

namespace UNITE.StepDefination
{
    [Binding]
    class PostDemo
    {
        BDDdriver DriverInstance;
        IAPIHelper APIDriver;
        JSONUtils JsonUtlis;
        SystemPathUtils SystemtPath;

        string uri;
        public PostDemo(BDDdriver bdd)
        {
            this.DriverInstance = bdd;
            APIDriver = new BaseDriverHelper(DriverInstance.GetAPIDriver());
            JsonUtlis = new JSONUtils();
            SystemtPath = new SystemPathUtils();
        }

        [Given(@"I have submit a SPQR request for ""(.*)""")]
        public void GivenIHaveSubmitASPQRRequestFor(string p0)
        {
            Console.WriteLine(p0);
            // GET the Base URI
            string uri_path = JsonUtlis.GetJsonValueByKey(SystemtPath.getProjectPath()+ @"\Resources\API\NodePath\Path.json", "UAT_url");
            uri = JsonUtlis.GetConfigValueByKey(uri_path);
            
            // Concat the base uri with endpoint
            uri = uri + "/api/v1";
            
            // Create the Post Request
            APIDriver.CreateRequest(Method.POST, uri);
            
            // Read the request from json file
            string requestBody = JsonUtlis.ReadRequestFromFile("SPQR_Request.json");
            
            // Update the Request 
            requestBody = JsonUtlis.UpdateRequestFromFile(requestBody, "\"surveyRequired\": \"N\"", "\"surveyRequired\": \"Y\"");
            
            // Add the request body
            APIDriver.AddBodyInRequest(RequestBodyType.JSON,requestBody);
            
            // Submit the request
            APIDriver.SubmitRequest();
        }

        [Then(@"Responce should contains the Operator Name as ""(.*)""")]
        public void ThenResponceShouldContainsTheServeyRequiredAs(string p0)
        {
            // Get the Json Path from Node Path File
            string name = JsonUtlis.GetJsonValueByKey(SystemtPath.getProjectPath() + @"\Resources\API\NodePath\Path.json", "NotificationData_Name");
            
            // Validate the request is sumbitted sucessfully
            APIDriver.AssertResponseIsSuccessful();

            // Validate the response has the given data
            APIDriver.AssertResponseBodyAttribute(name, p0);
        }

        public void Testing123(string p0)
        {
            /* PocoCourse items = JsonConvert.DeserializeObject<PocoCourse>(json);

             // Validate the response has the given data
             APIDriver.AssertResponseBodyAttribute(name, p0);*/
            
        }

    }
}

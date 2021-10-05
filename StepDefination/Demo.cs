using System;
using RestSharp;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using UNITE.Core;
using UNITE.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UNITE.StepDefination
{
    [Binding]
    class Demo
    {
        BDDdriver DriverInstance;
        IAPIHelper APIDriver;
        JSONUtils JsonUtlis;
        SystemPathUtils SystemtPath;
        string uri;
        public Demo(BDDdriver bdd)
        {
            this.DriverInstance = bdd;
            APIDriver = new BaseDriverHelper(DriverInstance.GetAPIDriver());
            JsonUtlis = new JSONUtils();
            SystemtPath = new SystemPathUtils();
        }

        [Given(@"I calls user api")]
        public void GivenICallsUserApi()
        {
            string uri_path = JsonUtlis.GetJsonValueByKey(SystemtPath.getProjectPath() + @"\Resources\API\NodePath\Path.json", "QA_url");
            uri = JsonUtlis.GetConfigValueByKey(uri_path);
        }

        [When(@"I pass the user id into user api")]
        public void WhenIPassTheUserIdIntoUserApi(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            int userid = (int)data.user_id;
            uri = uri + "/api/users/{id}";
            APIDriver.CreateRequest(Method.GET, uri);
            APIDriver.AddUrlSegment("id", userid);
            APIDriver.SubmitRequest();

        }

        [Then(@"I should see the user data according to the user id")]
        public void ThenIShouldSeeTheUserDataAccordingToTheUserId()
        {
            #region Ankit

            //APIDriver.AssertResponseIsSuccessful();
            Console.WriteLine("Ankit");
            //string uri = APIDriver.GetBaseURI("v1/home");
            string uri = APIDriver.GetBaseURI("url");
            //uri = uri + "v1/home";
            string city_id = "4487042";
            string key = "38dd3fd7ecdd48aa8b23c46684dbbad8";
            uri = uri + "current?city_id=" + city_id + "&key=" + key;
            //APIDriver.generatePayLoad();
            APIDriver.SetContentType("application/json");
            //APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            //APIDriver.UpdateRequestHeader("Cache-Control", "application/gzip");
            //APIDriver.UpdateRequestHeader("x-client-name", x_client_name);
            //APIDriver.UpdateRequestHeader("x-order-reference", x_order_reference);
            //APIDriver.UpdateRequestHeader("x-transaction-id", x_transaction_id);
            // Dictionary<String, String> headerMap = new Dictionary<String, String>();
            //headerMap.Add("supplierId", "358");
            //headerMap.Add("supplierCode", "SC101");
            //headerMap.Add("supplierName", "Nazim Khan");
            //headerMap.Add("adapterEndPoint", "v1//adapter");
            //APIDriver.addRequestParameters(headerMap);
            //APIDriver.addRequestParameters("supplierId", "358");
            //APIDriver.addRequestParameters("supplierCode", "SC101");
            //APIDriver.addRequestParameters("supplierName", "Nazim Khan");
            //APIDriver.addRequestParameters("supplierName", "v2//adapter");
            APIDriver.SubmitRequest(Method.GET, uri);
            string city_name = "Raleigh";
            APIDriver.AssertStringInResponseBody(city_name);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertHeaderAttribute("Content-Type", "application/json;");
            APIDriver.AssertResponseBodyAttribute("data[0].state_code", "NC");
            APIDriver.AssertResponseBodyAttribute("data[0].clouds", "25");

            #endregion

            #region Naved

            #region Used 
            
            #region TestRail

            uri = APIDriver.GetBaseURI("url");
            APIDriver.Authentication(AuthType.BASIC, "test360.233@gmail.com", "X5oAkVTNXCYG5mqP9cdj");

            string url = uri + $"index.php?/api/v2/add_project";
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"name\": \"This is a new project\",\"announcement\": \"This is the description for the project\",\"show_announcement\": true}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            string projectId = APIDriver.GetSingleValueFromResponse("id");

            url = uri + $"/index.php?/api/v2/get_project/{projectId}";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertStringInResponseBody("This is a new project");
            APIDriver.AssertResponseBodyAttribute("name", "This is a new project");
            APIDriver.AssertResponseBodyAttribute("id", projectId);

            APIDriver.ValidateResponseJsonSchema("{\"name\": \"This is new section\",\"description\": \"This is new section description\",\"parent_id\": null}");

            url = uri + $"index.php?/api/v2/add_suite/{projectId}";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"name\": \"This is a new Suite\",\"announcement\": \"This is the description for the suite\"}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            string suiteId = APIDriver.GetSingleValueFromResponse("id");

            url = uri + $"index.php?/api/v2/add_section/{projectId}";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"suite_id\":"+suiteId+",\"name\": \"This is new section\",\"description\": \"This is new section description\",\"parent_id\": null}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            string sectionId = APIDriver.GetSingleValueFromResponse("id");

            url = uri + $"index.php?/api/v2/get_section/{sectionId}";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("name", "This is new section");
            APIDriver.AssertResponseBodyAttribute("description", "This is new section description");
            APIDriver.AssertResponseBodyAttribute("id", sectionId);

            url = uri + $"index.php?/api/v2/update_section/{sectionId}";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"name\": \"This is new updated section\",\"description\": \"This is new updated section description\"}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            sectionId = APIDriver.GetSingleValueFromResponse("id");

            url = uri + $"index.php?/api/v2/get_section/{sectionId}";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("name", "This is new updated section");
            APIDriver.AssertResponseBodyAttribute("description", "This is new updated section description");
            APIDriver.AssertResponseBodyAttribute("id", sectionId);

            url = uri + $"index.php?/api/v2/add_case/{sectionId}";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"title\": \"This is a test case\",\"type_id\": 1,\"priority_id\": 3,\"estimate\": \"5m\",\"refs\": \"RF-1, RF-2\"}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            string caseId = APIDriver.GetSingleValueFromResponse("id");
            APIDriver.AssertResponseBodyAttribute("id", caseId);
            APIDriver.AssertResponseBodyAttribute("title", "This is a test case");
            APIDriver.AssertResponseBodyAttribute("priority_id", "3");
            APIDriver.AssertResponseBodyAttribute("estimate", "5m");

            url = uri + $"index.php?/api/v2/get_case/{caseId}";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("id", caseId);
            APIDriver.AssertResponseBodyAttribute("title", "This is a test case");
            APIDriver.AssertResponseBodyAttribute("priority_id", "3");
            APIDriver.AssertResponseBodyAttribute("estimate", "5m");

            url = uri + $"index.php?/api/v2/get_case/Invalid";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(400);
            APIDriver.AssertStatusLine("Bad Request");
            APIDriver.AssertResponseBodyAttribute("error", "Field :case_id is not a valid ID.");
            APIDriver.AssertNodeIsPresent("error",true);
            APIDriver.AssertNodeIsPresent("id",false);
            APIDriver.AssertHeaderAttribute("Content-Type", "application/json");
            APIDriver.AssertHeaderAttribute("Accept-Encoding", "gzip, deflate, br");

            url = uri + $"index.php?/api/v2/update_case/{caseId}";
            header = new Dictionary<string, string>();
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.SetContentType("application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"title\": \"This is updated test case title\",\"priority_id\": 2,\"estimate\": \"7m\",\"refs\": \"RF-1, RF-2\",\"custom_preconds\": \"This is pre-condition of TC\",\"custom_steps\": \"Step1\nStep2\nStep3\nStep4\",\"custom_expected\": \"Expected1\nExpected2\nExpected3\nExpected4\"}");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertResponseIsSuccessful();
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("id", caseId);
            APIDriver.AssertResponseBodyAttribute("title", "This is updated test case title");
            APIDriver.AssertResponseBodyAttribute("priority_id", "2");
            APIDriver.AssertResponseBodyAttribute("estimate", "7m");

            url = uri + $"index.php?/api/v2/delete_case/{caseId}";
            header = new Dictionary<string, string>();
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.SetContentType("application/json");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertResponseIsSuccessful();
            APIDriver.AssertStatusLine("OK");

            url = uri + $"index.php?/api/v2/get_case/{caseId}";
            APIDriver.SubmitRequest(Method.GET, url);
            APIDriver.AssertStatusCode(400);
            APIDriver.AssertStatusLine("Bad Request");
            APIDriver.AssertResponseBodyAttribute("error", "Field :case_id is not a ",true);

            url = uri + $"index.php?/api/v2/delete_section/{sectionId}";
            header = new Dictionary<string, string>();
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.SetContentType("application/json");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertResponseIsSuccessful();
            APIDriver.AssertStatusLine("OK");

            url = uri + $"index.php?/api/v2/delete_suite/{suiteId}";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
           
            url = uri + $"index.php?/api/v2/delete_project/{projectId}";
            header = new Dictionary<string, string>();
            header.Add("x-api-ident", "beta");
            APIDriver.AddRequestHeaders(header);
            APIDriver.SetContentType("application/json");
            APIDriver.SubmitRequest(Method.POST, url);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertResponseIsSuccessful();
            APIDriver.AssertStatusLine("OK");

            #endregion
            
            #endregion

            url = string.Empty;
            //string uri = string.Empty;

            url = APIDriver.GetBaseURI("url");
            uri = url + "/auth";
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"username\" : \"admin\",\"password\" : \"password123\"}");
            APIDriver.SubmitRequest(Method.POST, uri);
            APIDriver.ValidateResponseXMLSchema(@"C:\Users\naved.a\Downloads\demo.xsd");

            url = APIDriver.GetBaseURI("url");
            uri = url + "/auth";
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"username\" : \"admin\",\"password\" : \"password123\"}");
            APIDriver.SubmitRequest(Method.POST, uri);
            string token = APIDriver.GetSingleValueFromResponse("token");
            APIDriver.Authentication(AuthType.BEARER, token);
            Console.WriteLine(APIDriver.ReturnResponse().Contains(token));
            Console.WriteLine(APIDriver.ReturnResponse());

            uri = url + "/booking";
            Dictionary<String, String> headerMap = new Dictionary<String, String>();
            headerMap.Add("firstname", "sally");
            headerMap.Add("lastname", "brown");
            uri = APIDriver.AppendUriWithParameters(uri, headerMap);
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.SubmitRequest(Method.GET, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");

            uri = url + "/booking";
            header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("Accept", "application/json");
            APIDriver.SubmitRequestWithHeader(Method.GET, uri, header);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");

            uri = url + "/booking";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"firstname\" : \"Naved\",\"lastname\" : \"Ali\",\"totalprice\" : 111,\"depositpaid\" : true,\"bookingdates\" : {\"checkin\" : \"2021-01-01\",\"checkout\" : \"2021-07-08\"},\"additionalneeds\" : \"Breakfast\"}");
            APIDriver.SubmitRequest(Method.POST, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            string id = APIDriver.GetSingleValueFromResponse("bookingid");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.SubmitRequest(Method.GET, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("firstname", "Naved");
            APIDriver.AssertResponseBodyAttribute("lastname", "Ali");
            APIDriver.AssertResponseBodyAttribute("bookingdates.checkin", "2021-01-01");
            APIDriver.AssertResponseBodyAttribute("bookingdates.checkout", "2021-07-08");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.UpdateRequestHeader("Cookie", $"token={token}");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"firstname\" : \"NA\",\"lastname\" : \"AL\",\"totalprice\" : 123,\"depositpaid\" : true,\"bookingdates\" : {\"checkin\" : \"2021-10-10\",\"checkout\" : \"2021-12-12\"},\"additionalneeds\" : \"Breakfast\"}");
            APIDriver.SubmitRequest(Method.PUT, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.SubmitRequest(Method.GET, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("firstname", "NA");
            APIDriver.AssertResponseBodyAttribute("lastname", "AL");
            APIDriver.AssertResponseBodyAttribute("totalprice", "123");
            APIDriver.AssertResponseBodyAttribute("bookingdates.checkin", "2021-10-10");
            APIDriver.AssertResponseBodyAttribute("bookingdates.checkout", "2021-12-12");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.UpdateRequestHeader("Cookie", $"token={token}");
            APIDriver.AddBodyInRequest(RequestBodyType.JSON, "{\"firstname\" : \"Naved1\",\"lastname\" : \"Ali1\",\"totalprice\" : 222,\"depositpaid\" : false}");
            APIDriver.SubmitRequest(Method.PATCH, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.SubmitRequest(Method.GET, uri);
            APIDriver.AssertStatusCode(200);
            APIDriver.AssertStatusLine("OK");
            APIDriver.AssertResponseBodyAttribute("firstname", "Naved1");
            APIDriver.AssertResponseBodyAttribute("lastname", "Ali1");
            APIDriver.AssertResponseBodyAttribute("totalprice", "222");
            APIDriver.AssertResponseBodyAttribute("depositpaid", "False");

            uri = url + $"/booking/{id}";
            APIDriver.UpdateRequestHeader("Content-Type", "application/json");
            APIDriver.UpdateRequestHeader("Accept", "application/json");
            APIDriver.UpdateRequestHeader("Cookie", $"token={token}");
            APIDriver.SubmitRequest(Method.DELETE, uri);
            APIDriver.AssertStatusCode(201);
            APIDriver.AssertStatusLine("Created");

            #endregion

        }

        [When(@"I Test someting")]
        public void WhenITestSometing()
        {
            string val = "https://restful-booker.herokuapp.com/booking/{bookingDid}";
            var groups = Regex.Match(val, @"\{(.*?)\}").Groups;
            var x1 = groups[1].Value;
            Console.WriteLine(x1);
        }

    }
}
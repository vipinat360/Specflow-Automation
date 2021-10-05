using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RestSharp;
using UNITE.Utils;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using Authlete.Util;
using System.Numerics;
using RestSharp.Authenticators;
using System.Xml.Schema;
using System.Xml.Linq;
using Newtonsoft.Json.Schema;

/*
 * Created By: Chirag
*/
namespace UNITE.Core
{
    class BaseDriverHelper : IAPIHelper
    {
        IRestResponse response;
        RestRequest request;
        RestClient restClient;
        JSONUtils jSONUtils;
        public static string Payload = "";
        string Root = "Resources\\API\\apiResourceTemplate\\";
        public static JObject JSONDocumentPayload;
        public static XmlDocument XMLDocumentPayload;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restClient"></param>
        public BaseDriverHelper(RestClient restClient)
        {
            this.restClient = restClient;
            jSONUtils = new JSONUtils();
            this.request = new RestRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requesttype"></param>
        /// <param name="body"></param>
        public void AddBodyInRequest(RequestBodyType requestBodyType, object body)
        {
            switch (requestBodyType)
            {
                case RequestBodyType.JSON:
                    if (!body.ToString().Contains("\\n") && body.ToString().Contains("\n"))
                    {
                        body = body.ToString().Replace("\n", "\\n");
                    }
                    this.request.AddJsonBody(body);
                    break;
                case RequestBodyType.XML:
                    this.request.AddXmlBody(body);
                    break;
                default:
                    BDDdriver.AddLog("Please select correct body type");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestRequest AddUrlSegment(string name, object value)
        {
            this.request.AddUrlSegment(name, value);
            return this.request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerMap"></param>
        public void AddRequestHeaders(Dictionary<String, String> headerMap)
        {
            this.request.AddHeaders(headerMap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerMap"></param>
        [Obsolete]
        public void AddRequestParameters(Dictionary<String, String> parameterMap)
        {
            foreach (string key in parameterMap.Keys)
            {
                this.request.AddParameter(new Parameter(key, parameterMap[key], ParameterType.RequestBody));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HeaderKey"></param>
        /// <param name="value"></param>
        public void AddRequestParameters(string HeaderKey, object value)
        {
            this.request.AddParameter(HeaderKey, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headerMap"></param>
        /// <returns></returns>
        public string AppendUriWithParameters(string uri, Dictionary<String, String> headerMap)
        {
            int i = 1;
            foreach (string key in headerMap.Keys)
            {
                if (i == 1)
                {
                    uri = uri + "?" + key + "=" + headerMap[key];
                }
                else
                {
                    uri = uri + "&" + key + "=" + headerMap[key];
                }
                i++;
            }
            return uri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expectedvalue"></param>
        public void AssertResponseBodyAttribute(string path, string expectedvalue, bool partialMatch = true)
        {
            string ContentType = this.response.ContentType;
            string actual = null;
            if (ContentType.Contains("application/json"))
            {
                actual = jSONUtils.GetJsonValueFromString(path, this.response.Content);
            }
            BDDdriver.AddLog("actual " + actual + " :::: expectedvalue " + expectedvalue);
            if (partialMatch)
                Assert.AreEqual(true, actual.Contains(expectedvalue));
            else
                Assert.AreEqual(expectedvalue, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AssertResponseIsSuccessful()
        {
            BDDdriver.AddLog($"Got response from server {this.response.Content}");
            Boolean flag = this.response.IsSuccessful;
            BDDdriver.AddLog(this.response.Content);
            BDDdriver.AddLog(this.response.StatusCode);
            Assert.IsTrue(flag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpectedData"></param>
        public void AssertStringInResponseBody(string ExpectedData)
        {
            string responseBody = this.response.Content.ToString();
            BDDdriver.AddLog("Response Body is: " + responseBody);
            Assert.AreEqual(responseBody.Contains(ExpectedData), true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpectedStatusCode"></param>
        public void AssertStatusCode(int ExpectedStatusCode)
        {
            HttpStatusCode Code = this.response.StatusCode;
            int statusCode = (int)(Code);
            Assert.AreEqual(ExpectedStatusCode, statusCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExpectedStatusLine"></param>
        public void AssertStatusLine(string ExpectedStatusLine)
        {
            string statusLine = this.response.StatusDescription;
            BDDdriver.AddLog("Status Line is: " + statusLine);
            Assert.AreEqual(statusLine, ExpectedStatusLine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HeaderName"></param>
        /// <param name="ExpectedheaderValue"></param>
        public void AssertHeaderAttribute(string HeaderName, string ExpectedheaderValue)
        {
            string headers = null;
            string[] header;
            try
            {
                headers = this.response.Headers.ToList()
                                 .Find(x => x.Name == HeaderName)
                                 .Value.ToString();
                BDDdriver.AddLog(headers);
                header = headers.Split(";");
                if (header[0].ToString().Equals(ExpectedheaderValue))
                {
                    Assert.True(header[0].Equals(ExpectedheaderValue), "Actual Header value " + header[0] + " matched/contains expected Header value " + ExpectedheaderValue);
                }
                else
                {
                    Assert.Fail("Header value not matched!");
                }
            }
            catch (NullReferenceException e)
            {
                e.Message.ToString();
                BDDdriver.AddLog(e.Message.ToString());
            }
            catch (Exception e)
            {
                e.Message.ToString();
                BDDdriver.AddLog(e.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="Expectedvalue"></param>
        public void AssertResponseBodyAttribute(string Node, string Expectedvalue)
        {
            string headers = null;
            string[] header;
            BDDdriver.AddLog("Response body is " + this.response.Content.ToString());

            headers = this.response.Headers.ToList()
                                  .Find(x => x.Name == "Content-Type")
                                  .Value.ToString();
            BDDdriver.AddLog(headers);
            header = headers.Split(";");
            if (header[0].Contains("json"))
            {
                string nodeValue = string.Empty;
                var token = JToken.Parse(this.response.Content);
                if (token is JObject)
                    nodeValue = (string)JObject.Parse(this.response.Content).SelectToken(Node);
                else
                    nodeValue = (string)JArray.Parse(this.response.Content).SelectToken(Node);

                BDDdriver.AddLog("The value of " + Node + " is: " + nodeValue);
                BDDdriver.AddLog("Expected Value is : " + Expectedvalue);
                BDDdriver.AddLog("Condition Value is " + nodeValue.ToLower().Contains(Expectedvalue.ToLower()));
                Assert.True(nodeValue.ToLower().Contains(Expectedvalue.ToLower()),
                            "Expected the Value of <b>" + Node + "</b> Attribute value contains <b>" + Expectedvalue.ToLower()
                                    + "</b> but  Actual returned was <b>" + nodeValue.ToLower() + "</b>");
            }
            else if (header[0].Contains("xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.response.Content);
                Assert.True(doc.DocumentElement.SelectSingleNode(Node).InnerText.Equals(Expectedvalue),
                               "Expected the Value of <b>" + Node + "</b> Attribute value contains <b>" + Expectedvalue
                                + "</b> but  Actual returned was <b>" + doc.DocumentElement.SelectSingleNode(Node).InnerText + "</b>");
            }
            else
            {
                Assert.Fail("Response Content-Type is not matched.Please check the Assertion");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="expected"></param>
        public void AssertNodeIsPresent(string Node, bool expected)
        {
            bool flag = false;
            if (this.response.Headers.ToList().Find(x => x.Name == "Content-Type").Value.ToString().Contains("json"))
            {
                try
                {
                    var token = JToken.Parse(this.response.Content);
                    if (token is JObject)
                    {
                        if (JObject.Parse(this.response.Content).SelectTokens(Node).ToList().Count() > 0)
                        {
                            if (!JObject.Parse(this.response.Content).SelectToken(Node).Contains("null"))
                            {
                                flag = true;
                            }
                        }
                    }
                    else
                    {
                        if (JArray.Parse(this.response.Content)[0].SelectTokens(Node).ToList().Count() > 0)
                        {
                            if (!JArray.Parse(this.response.Content).SelectToken(Node).Contains("null"))
                            {
                                flag = true;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    e.Message.ToString();
                }
                Assert.AreEqual(flag, expected, "Node <b>" + Node + "</b> contains in the response ");
            }
            else if (this.response.Headers.ToList().Find(x => x.Name == "Content-Type").Value.ToString().Contains("xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.response.Content);
                try
                {
                    if (doc.SelectNodes(Node).Count > 0)
                    {
                        flag = true;
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    e.Message.ToString();
                }
                Assert.AreEqual(flag, expected,
                    "Node <b>" + Node + "</b> contains in the response which is expected :- " + expected);
            }
            else
            {
                Assert.Fail("Response Content-Type is not matched.Please check the Assertion");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Authentication(AuthType authType, string username, string password = "")
        {
            switch (authType)
            {
                case AuthType.BASIC:
                    this.restClient.Authenticator = new HttpBasicAuthenticator(username, password);
                    break;
                case AuthType.OAUTH2:
                    this.restClient.Authenticator = new OAuth2UriQueryParameterAuthenticator(username);
                    break;
                case AuthType.BEARER:
                    //this.request.AddHeader("Authorization", $"Bearer {username}");
                    this.restClient.AddDefaultHeader("Authorization", $"Bearer {username}");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Base64Encoder(string data)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedString = System.Convert.ToBase64String(plainTextBytes);
            BDDdriver.AddLog("Base code Encoded String:::::" + encodedString);
            return encodedString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public string Base64Decoder(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            string decodedString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            BDDdriver.AddLog("Decoded string::::::" + decodedString);
            return decodedString;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public void BooleanComparator(Boolean actual, Boolean expected)
        {
            BDDdriver.AddLog("Actual string : " + actual);
            BDDdriver.AddLog("Expected string : " + expected);
            Assert.IsTrue(actual == expected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateOrderNumber()
        {
            //Timestamp timestamp = new Timestamp(System.currentTimeMillis());
            //string orderNum = "" + timeStamp;
            DateTime date = DateTime.Now;
            string orderNum = "" + date.TimeOfDay.Ticks;
            return orderNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public RestRequest CreateRequest(Method method, string uri)
        {
            this.request = new RestRequest(uri, method);
            return this.request;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GeneratePayLoad()
        {
            this.request.AddJsonBody(Payload);
            BDDdriver.AddLog("********************************************");
            BDDdriver.AddLog(Payload);
            BDDdriver.AddLog("********************************************");
            Payload = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public string GetBaseURI(string uri)
        {
            string endpoint = null;
            string file = null;
            TextReader reader = null;
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            try
            {
                file = @$"{path}\config.properties";
                IDictionary<string, string> properties;

                reader = new StreamReader(file);
                properties = PropertiesLoader.Load(reader);
                if (properties["ENV"].Equals("local"))
                {
                    endpoint = properties["localmock_url"];
                }
                else
                {
                    string env = properties["ENV"];
                    endpoint = env.ToLower() + "_" + uri;
                    endpoint = properties[endpoint];
                }
            }
            catch (Exception e)
            {
                BDDdriver.AddLog(e.Message.ToString());
                e.StackTrace.ToString();

            }
            finally
            {
                try
                {
                    reader.Close();
                }
                catch (IOException e)
                {
                    e.StackTrace.ToString();
                }
            }
            return endpoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetResponse()
        {
            return this.response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public string GetSingleValueFromResponse(string Node)
        {
            string nodevalue = null;
            if (this.response.Headers.ToList().Find(x => x.Name == "Content-Type").Value.ToString().Contains("json"))
            {
                var token = JToken.Parse(this.response.Content);
                if (token is JObject)
                    nodevalue = (string)JObject.Parse(this.response.Content).SelectToken(Node);
                else
                    nodevalue = (string)JArray.Parse(this.response.Content).SelectToken(Node);
            }
            else if (this.response.Headers.ToList().Find(x => x.Name == "Content-Type").Value.ToString().Contains("xml"))
            {
                XmlDocument doc = null;
                try
                {
                    doc = new XmlDocument();
                    doc.Load(this.response.Content);
                }
                catch (Exception e)
                {
                    e.StackTrace.ToString();
                }
                nodevalue = doc.DocumentElement.SelectSingleNode(Node).InnerText;
            }
            else
            {
                Assert.Fail("Invalid Content Type");
            }
            return nodevalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Value"></param>
        public void GenerateMultipart(string fileName, string fileFullPath)
        {
            this.request.AddHeader("Content-Type", "multipart/form-data");
            this.request.AddFile(fileName, fileFullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public string GetTodaysDateInFormat(string dateFormat = "dd/MM/yyyy")
        {
            string requiredDate = DateTime.Today.ToString(dateFormat);
            return requiredDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDateString"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public bool IsValidDateFormat(string inputDateString, string dateFormat)
        {
            DateTime tempDate;
            try
            {
                bool validDate = DateTime.TryParseExact(inputDateString, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatefile"></param>
        /// <param name="Node"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public JObject InputGenerator(JObject templatefile, string Node, string Value)
        {
            JObject jsonObjectnodelist = null;
            JObject jsonObjectfinal = templatefile;
            String[] nodelist = Node.Split(".");
            if (nodelist.Length == 1)
            {
                // Update the Node
                BDDdriver.AddLog(jsonObjectnodelist.GetValue(nodelist[nodelist.Length - 1]));
            }
            else
            {
                for (int i = 0; i < nodelist.Length - 2; i++)
                {
                    jsonObjectnodelist = (JObject)templatefile.GetValue(nodelist[i]);
                    BDDdriver.AddLog(jsonObjectnodelist.ToString());
                }
                BDDdriver.AddLog(jsonObjectnodelist.GetValue(nodelist[nodelist.Length - 1]));
            }
            return templatefile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public void JsonComparator(JsonElement actual, JsonElement expected)
        {
            BDDdriver.AddLog("Actual string : " + actual);
            BDDdriver.AddLog("Expected string : " + expected);
            Assert.IsTrue(actual.Equals(expected));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public void PartialStringComparator(string actual = "", string expected = "")
        {
            BDDdriver.AddLog("Actual string : " + actual);
            BDDdriver.AddLog("Expected string : " + expected);
            Assert.IsTrue(actual.ToLower().Contains(expected.ToLower()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReturnPaylod()
        {
            return Payload;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReturnResponse()
        {
            return this.response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestpath"></param>
        public void ReadPayload(string requestpath)
        {
            try
            {
                string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                string Filename = @$"{path}\" + Root + requestpath;
                string payload = ReadRequestTemplate(Filename);
                Payload = payload;
            }
            catch (IOException e)
            {
                e.StackTrace.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadRequestTemplate(string path)
        {
            string readTex = File.ReadAllText(path);
            return readTex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentTypeRestAssured(DataFormat contentType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public string SaveAttributeValue(string Node)
        {
            string headers = null;
            string[] header;
            string Nodevalue = null;
            BDDdriver.AddLog("Respoence body is " + this.response.Content.ToString());
            headers = this.response.Headers.ToList()
                                 .Find(x => x.Name == "Content-Type")
                                 .Value.ToString();
            BDDdriver.AddLog(headers);
            header = headers.Split(";");
            if (header[0].Contains("json"))
            {
                BDDdriver.AddLog("Data in Reponce is" + this.response.Content.ToString());
                Nodevalue = (String)JObject.Parse(this.response.Content).SelectToken(Node);
                BDDdriver.AddLog("The value of " + Node + " is: " + Nodevalue);
                Assert.AreEqual(Nodevalue, Nodevalue);
            }
            else if (header[0].Contains("xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.response.Content);
                Nodevalue = doc.DocumentElement.SelectSingleNode(Node).InnerText;
            }
            else
            {
                Assert.Fail("Invalid Content Type");
            }
            return Nodevalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="URI"></param>
        public void SubmitRequest(Method method, string URI)
        {
            this.response = null;
            BDDdriver.AddLog("Submited URI : "+URI);
            this.request.Method = method;
            this.request.Resource = URI;
            this.response = this.restClient.Execute(this.request);
            BDDdriver.AddLog($"Response Body After {method} Request: " + this.response.Content.ToString());
            //this.request.Body = null;
            //We had to this as after POST request if we are using GET then request is carring NULL body and it was giving error so re-initialize the request object
            this.request = new RestRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            this.request.AddHeader("Content-Type", contentType);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SubmitRequest()
        {
            this.response = null;
            this.response = this.restClient.Execute(this.request);
            BDDdriver.AddLog($"Response Body After {this.request.Method} Request: " + this.response.Content.ToString());
            this.request = new RestRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="URI"></param>
        public void SubmitRequestWithHeader(Method method, string URI, Dictionary<string, string> headerMap)
        {
            this.response = null;
            AddRequestHeaders(headerMap);
            this.request = new RestRequest(URI, method);
            this.response = this.restClient.Execute(this.request);
            BDDdriver.AddLog($"Response Body After {method} Request: " + this.response.Content.ToString());
            this.request = new RestRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public void StringComparator(string actual, string expected)
        {
            BDDdriver.AddLog("Actual string : " + actual);
            BDDdriver.AddLog("Expected string : " + expected);
            if (expected == null)
            {
                expected = "";
            }
            if (actual == null)
            {
                actual = "";
            }
            Assert.IsTrue(actual.Equals(expected));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HeaderKey"></param>
        /// <param name="value"></param>
        public void UpdateRequestHeader(string HeaderKey, string value)
        {
            this.request.AddHeader(HeaderKey, value);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        public void UpdateAttributeInRequestBody(string Filename, string Key, string value)
        {
            string jsonString = null;
            if (Filename.ToLower().EndsWith(".json"))
            {
                String abc = "";
                String[] arrOfStr = Key.Split("//");
                if (Key.EndsWith("//"))
                {
                    BDDdriver.AddLog("Provided Json node path is not correct!! it can't be end with //");
                    Assert.Fail("Provided Json node path is not correct!! it can't be end with //");
                }
                string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                try
                {
                    Filename = @$"{path}\" + Root + Filename;
                    BDDdriver.AddLog();
                    jsonString = File.ReadAllText(Filename);
                }
                catch (Exception e)
                {
                    BDDdriver.AddLog("File path Pointed to Json file is incorrect!!! Please provide a valid path... ");
                    BDDdriver.AddLog(e.Message);
                }
                JObject jsonObject = JObject.Parse(jsonString);
                JObject idObj1 = null;

                if (arrOfStr.Length == 1 || arrOfStr.Length == 0)
                {
                    try
                    {
                        if (!arrOfStr[0].Contains("//"))
                        {
                            BDDdriver.AddLog("// is missing with Node value!!!!");
                            Assert.False(arrOfStr[0].Contains("//"), "// is missing with Node value!!!!");

                        }
                        else
                        {
                            BDDdriver.AddLog("Node is not correctly provided!!!!");
                        }
                    }
                    catch (Exception e)
                    {
                        BDDdriver.AddLog("Node is not correctly provided!!!!");
                        BDDdriver.AddLog(e.Message);
                    }
                }
                else
                {
                    for (int i = 0; i < arrOfStr.Length - 2; i++)
                    {
                        if (idObj1 == null)
                        {
                            idObj1 = ((JObject)jsonObject.GetValue(arrOfStr[i + 1]));
                        }
                        else
                        {
                            idObj1 = (JObject)idObj1.GetValue(arrOfStr[i + 1]);
                        }
                    }
                    if (arrOfStr.Length == 2)
                    {
                        jsonObject.Add(arrOfStr[arrOfStr.Length - 1], value);
                    }
                    else
                    {
                        try
                        {
                            idObj1[arrOfStr[arrOfStr.Length - 1]] = value;
                            BDDdriver.AddLog("\n idobj1 jsn value:::" + idObj1.ToString());
                            BDDdriver.AddLog("\n jsonObject jsn value:::" + jsonObject.ToString());
                        }
                        catch (Exception e)
                        {
                            BDDdriver.AddLog(e.Message.ToString());
                        }
                    }
                    JSONDocumentPayload = jsonObject;
                    abc = jsonObject.ToString();
                }
                Payload = abc;
                BDDdriver.AddLog("Updated Paylod" + Payload);
            }
            else if (Filename.ToLower().EndsWith(".xml"))
            {
                //XmlTextReader textReader = new XmlTextReader(Directory.GetCurrentDirectory() + "/" + Root + Filename);
                XmlDocument doc = new XmlDocument();
                doc.Load(Directory.GetCurrentDirectory() + "/" + Root + Filename);
                doc.DocumentElement.SelectSingleNode(Key).InnerText = value.ToString();
                XMLDocumentPayload = doc;
                Payload = doc.DocumentElement.SelectSingleNode(Key).OuterXml;
                BDDdriver.AddLog("Updated Paylod" + Payload);
            }
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="values"></param>
        public void UpdateAttributeInRequestBody(string Filename, Dictionary<string, string> values)
        {
            if (Filename.ToLower().EndsWith(".json"))
            {
                // TODO need to implement
            }
            else if (Filename.ToLower().EndsWith(".xml"))
            {
                XmlDocument doc = new XmlDocument();
                string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                Filename = @$"{path}\" + Root + Filename;
                doc.Load(Filename);
                foreach (string key in values.Keys)
                {
                    doc.DocumentElement.SelectSingleNode(key).InnerText = values[key];
                }
                Payload = doc.OuterXml;
                BDDdriver.AddLog("Updated Paylod" + Payload);
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="Value"></param>
        public void UpdatedAttributeInXMLPayload(string Node, string Value)
        {
            XmlDocument doc = XMLDocumentPayload;
            doc.DocumentElement.SelectSingleNode(Node).InnerText = Value;
            XMLDocumentPayload = doc;
            Payload = doc.OuterXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="Value"></param>
        /// <param name="isstring"></param>
        public void UpdatedAttributeInJSONPayload(string Node, string Value, bool isstring)
        {
            string abc = "";
            String[] arrOfStr = Node.Split("//");

            if (Node.EndsWith("//"))
            {
                BDDdriver.AddLog("Provided Json node path is not correct!! it can't be end with //");
                Assert.Fail("Provided Json node path is not correct!! it can't be end with //");
            }
            JObject jsonObject = JSONDocumentPayload;
            JObject idObj1 = null;
            if (arrOfStr.Length == 1 || arrOfStr.Length == 0)
            {
                try
                {
                    if (!arrOfStr[0].Contains("//"))
                    {
                        BDDdriver.AddLog("// is missing with Node value!!!!");
                        Assert.False(arrOfStr[0].Contains("//"), "// is missing with Node value!!!!");
                    }
                    else
                    {
                        BDDdriver.AddLog("Node is not correctly provided!!!!");
                    }
                }
                catch (Exception e)
                {
                    BDDdriver.AddLog("Node is not correctly provided!!!!");
                    e.Message.ToString();
                }
            }
            else
            {
                for (int i = 0; i < arrOfStr.Length - 2; i++)
                {

                    // System.out.println(arrOfStr[i+1]);
                    if (idObj1 == null)
                    {
                        idObj1 = ((JObject)(jsonObject.GetValue(arrOfStr[i + 1])));
                    }
                    else
                    {
                        idObj1 = (JObject)idObj1.GetValue(arrOfStr[i + 1]);
                    }
                }

                if (arrOfStr.Length == 2)
                {
                    if (isstring)
                        jsonObject.Add(arrOfStr[arrOfStr.Length - 1], Value);
                    else
                    {

                        BigInteger obj = BigInteger.Parse(Value);
                        jsonObject.Add(arrOfStr[arrOfStr.Length - 1], JToken.FromObject(obj));
                    }
                }
                else
                {
                    if (isstring)
                        idObj1.Add(arrOfStr[arrOfStr.Length - 1], Value);
                    else
                    {
                        BigInteger obj = BigInteger.Parse(Value);
                        idObj1.Add(arrOfStr[arrOfStr.Length - 1], JToken.FromObject(obj));
                    }

                }
                JSONDocumentPayload = jsonObject;
                abc = jsonObject.ToString();
            }

            Payload = abc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void ValidateResponseJsonSchema(string fileName)
        {
            bool isCorrect = false;
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            fileName = $"{path}/{Root}/{fileName}";

            JSchema schema = JSchema.Parse(File.ReadAllText(fileName));
            schema.AllowAdditionalProperties = false;
            var obj = JObject.Parse(this.response.Content);
            
            isCorrect = obj.IsValid(schema);

            Assert.AreEqual(true, isCorrect, $"Response is not matched with given JSON schema");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void ValidateResponseXMLSchema(string fileName)
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            fileName = $"{path}/{Root}/{fileName}";
            
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", fileName);

            string tempFile = "Demo.xml";

            string responseContent = this.response.Content;
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(tempFile))
            {
                sw.WriteLine(responseContent);
            }

            XmlReader rd = XmlReader.Create(tempFile);
            XDocument doc = XDocument.Load(rd);
            doc.Validate(schema, ValidationEventHandler);
        }

        private void ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void ValidateResponseXMLSchemaWithoutSoapEnvelope(string path)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using System.Text.Json;
using Newtonsoft.Json.Linq;

/*
 *  Created By: Chirag
 */
namespace UNITE.Core
{
	public enum RequestBodyType
	{
		JSON,
		XML
	}

	public enum AuthType
	{
		BASIC,
		OAUTH2,
		BEARER
	}

	interface IAPIHelper
	{
		void AddRequestHeaders(Dictionary<String, String> headerMap);

		/**
		 * This method is to read the input requests from parameter.
		 *
		 * @param paramsMap - Map of parameters.
		 * @throws IOException
		 */
		void AddRequestParameters(Dictionary<String, String> paramsMap);

		void AddRequestParameters(string HeaderKey, object Value);

		void AddBodyInRequest(RequestBodyType requestBodyType, object body);

		RestRequest AddUrlSegment(string name, object value);

		void AssertResponseIsSuccessful();

		/**
	 * Append URI with parameters
	 *
	 * @param uri
	 * @param map for uri
	 * @author chirag.s
	 */
		string AppendUriWithParameters(string uri, Dictionary<String, String> headerMap);

		/**
		 * Assert an string to be contains on the response Body
		 *
		 * @param ExpectedData - Expected string to contains in response body.
		 *                     Nothing to Return
		 */
		void AssertStringInResponseBody(string ExpectedData);

		/**
		 * Assert Status code of the Response
		 *
		 * @param ExpectedStatusCode - Expected Status code return by Service call.
		 *                           Nothing to Return
		 */
		void AssertStatusCode(int ExpectedStatusCode);

		/**
		 * Assert Status line of the Response
		 *
		 * @param ExpectedStatusLine - Expected Status line return by Service call.
		 *                           Nothing to Return
		 */
		void AssertStatusLine(string ExpectedStatusLine);

		/**
		 * Assert specific header key value in the response.
		 *
		 * @param HeaderName          - Name of the HeaderKey to be Verified.
		 * @param ExpectedheaderValue - Expected value for the HeaderKey return by Service call.
		 *                            Nothing to Return
		 */
		void AssertHeaderAttribute(string HeaderName, string ExpectedheaderValue);

		/**
		 * Assert specific Body Key value in the response.
		 *
		 * @param Node          - Name of the Body Key to be Verified.
		 * @param Expectedvalue - Expected value for the Body Key return by Service call.
		 *                      Nothing to Return
		 * @throws ParserConfigurationException
		 * @throws IOException
		 * @throws SAXException
		 * @throws DocumentException
		 */
		void AssertResponseBodyAttribute(string Node, string Expectedvalue, bool partialMatch = true);

		/**
		 * Assert specific Body Key value in the response.
		 *
		 * @param Node          - Name of the Body Key to be Verified.
		 * @param Expectedvalue - Expected value for the Body Key return by Service call.
		 *                      Nothing to Return
		 * @throws ParserConfigurationException
		 * @throws IOException
		 * @throws SAXException
		 * @throws DocumentException
		 */
		void AssertResponseBodyAttribute(string Node, string Expectedvalue);

		/**
		 * @param Node
		 * @param Expectedvalue
		 * @throws SAXException
		 * @throws IOException
		 * @throws ParserConfigurationException
		 * @throws DocumentException
		 * @author chirag.s
		 */
		void AssertNodeIsPresent(string Node, bool expected);

		/**
		* Method to Authenticate the Requests.
		*
		* @param type     - This the type of Authentication used by Service. for Example <b>"Basic"</b>,<b>"preemptive"</b> etc.
		* @param username - This user name.
		* @param password - This password or any secret key ot Token key which required to authenticate.
		*/
		void Authentication(AuthType authType, string username, string password = "");

		/**
		 * Compait two strings
		 *
		 * @param actual   Actual boolean need to to validate
		 * @param expected expected bollean againt which validation need to be perform
		 * @author ashwnai.S
		 */
		void BooleanComparator(Boolean actual, Boolean expected);

		/**
		 * Returns the Encoded Data into Base64Formate
		 *
		 * @param data Data to be encoded
		 * @return encoded data
		 * @author ashwnai.S
		 */
		string Base64Encoder(string data);

		/**
		 * Returns the Decoded Data into Base64Formate
		 *
		 * @param data Data to be encoded
		 * @return encoded data
		 * @author ashwnai.S
		 */
		string Base64Decoder(string data);

		RestRequest CreateRequest(Method method, string uri);

		/**
		 * @author Dipesh.J
		 * @return orderNumber
		 */
		public string CreateOrderNumber();

		/**
		   * @author Dipesh.J
		   * @param dateFormat
		   */
		public string GetTodaysDateInFormat(string dateFormat);

		/**
		 * Submit a Form Data into the Request	
		 *
		 * @param key   Name of the Key needs to be passed as form data
		 * @param Value Value of the form key
		 * @author ashwnai.S
		 */
		public void GenerateMultipart(string fileName, string fileFullPath);

		/**
 * Generate the Final Pay load with the updated input data.
 * Nothing to Return
 */
		void GeneratePayLoad();

		/**
		 * Get Base URI
		 *
		 * @param uri
		 * @author chirag.s
		 */
		string GetBaseURI(string uri);

		/**
		 * @param node
		 * @author chirag.s
		 */
		string GetSingleValueFromResponse(string node);

		/**
		 * JSON input generator
		 *
		 * @param templatefile - JSON input file Template file path.
		 * @param Node         - Name of the node needs to be updated into Input Body
		 * @param Value        - New Value of the Node.
		 */
		JObject InputGenerator(JObject templatefile, string Node, string Value);

		/**
		 * @author Dipesh.J
		 * @param dateStr
		 * @param dateFormat
		 */
		public bool IsValidDateFormat(string dateStr, string dateFormat);

		/**
				* Exact Compair two Json object
		*
				* @param actual   Actual string need to to validate
		* @param expected expected string againt which validation need to be perform
		* @author ashwnai.S
		*/
		public void JsonComparator(JsonElement actual, JsonElement expected);

		/**
		 * Parial Compairison two strings
		 *
		 * @param actual   Actual string need to to validate
		 * @param expected expected string againt which validation need to be perform
		 * @author ashwnai.S
		 */
		public void PartialStringComparator(string actual, string expected);

		/**
		 * This method is to read the input requests template which latter used to generate the pay load.
		 *
		 * @param path - Path of the Input template file
		 * @throws IOException
		 */
		string ReadRequestTemplate(string path);

		/**
		 * Read Pay load from file
		 *
		 * @param path
		 * @author chirag.s
		 */
		void ReadPayload(string Requestpath);

		/**
		 * Returns the Current Payload
		 *
		 * @return Retuns the current payload
		 * @author ashwnai.s
		 */
		string ReturnPaylod();

		/**
		 * Return the Current Response
		 *
		 *
		 * @author ashwnai.S
		 */
		public string ReturnResponse();

		/**
		 * @param actual
		 * @param expected
		 * @author ashwani.s
		 */
		void StringComparator(string actual, string expected);

		/**
		 * @param method
		 * @param URI
		 * @author dipesh.j
		 */
		void SubmitRequestWithHeader(Method method, string URI, Dictionary<string, string> headerMap);

		void SubmitRequest();

		/** Submit the Request to Server with Specified Resource and Method.
		 * @param method - Name of the Methode to be used. for Example <b> GET</B>, <b> POST</B> etc.
		 * @param URI - Service URI. For Example - <b>"/Create"</b>
		 * Nothing to Return
		 */
		void SubmitRequest(Method method, string URI);

		/**
		 * Store specific Body Key returned in response.
		 *
		 * @param Node - Name of the Body Key to be Verified.
		 * @return a string value which conatns the Value of Node in s
		 */
		string SaveAttributeValue(string Node);

		void SetContentType(string contentType);

		void SetContentTypeRestAssured(DataFormat contentType);

		/**
		 * Method to update the Request Header Attributes.
		 *
		 * @param HeaderKey - This is the name of the header key needs to be updated.
		 * @param Value     - This is the new value which needs to be updated for the Header key.
		 */
		void UpdateRequestHeader(string HeaderKey, string Value);

		/**
		 * This method is update the attribute of the a key into the Request template.
		 *
		 * @param Filename - Path of the Input template file
		 * @param Key      - Name of the Key/NODE in case of JSON and XML.<b> "Customer.Name. FirstName"</b>
		 * @param Value    - Updated value of the Attribute.
		 * @throws IOException
		 * @throws ParserConfigurationException
		 * @throws SAXException
		 * @throws Exception
		 */
		void UpdateAttributeInRequestBody(string Filename, string Key, string Value);

		/**
		 * @param Filename
		 * @param values   : dictionary with xml node as key and values
		 * @throws IOException
		 * @throws SAXException
		 * @throws ParserConfigurationException
		 * @throws Exception
		 */
		void UpdateAttributeInRequestBody(string Filename, Dictionary<String, String> values);

		/**
		 * @param Node
		 * @param Value
		 * @author ashwani.s
		 */
		void UpdatedAttributeInXMLPayload(string Node, string Value);

		/**
		 * @param Node
		 * @param Value
		 * @author ashwani.s
		 */
		void UpdatedAttributeInJSONPayload(string Node, string Value, bool isstring);

		/**
		 * Validate Response Structure
		 *
		 * @param path
		 * @author chirag.s
		 */
		void ValidateResponseJsonSchema(string path);

		/**
		 * Validate Response Structure
		 *
		 * @param path
		 * @author chirag.s
		 */
		void ValidateResponseXMLSchema(string path);

		/**
		 * Validate Response Structure
		 *
		 * @param path
		 * @throws IOException
		 * @throws JAXBException
		 * @author Dipesh.Jain
		 */
		void ValidateResponseXMLSchemaWithoutSoapEnvelope(string path);
	}
}
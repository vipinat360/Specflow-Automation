using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UNITE.Utils
{
    class JSONUtils
    {
        public string GetJsonValueByKey(string path,string jpath)
        {
            string value=null;
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(path));
                value = (string)o1.SelectToken(jpath);
            }
            catch (Exception e)
            {
                value = "";
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return value;
        }

        public string GetJsonValueFromString(string jpath, string response)
        {
            string value = null;
            try
            {
                JObject o1 = JObject.Parse(response);
                value = (string)o1.SelectToken(jpath);
            }
            catch (Exception e)
            {
                value = "";
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return value;
        }

        public string GetConfigValueByKey(string Jpath)
        {
            SystemPathUtils obj = new SystemPathUtils();
            string path = obj.getProjectPath()+"Config.json";
            return GetJsonValueByKey(path, Jpath);
        }
        public string ReadRequestFromFile(string filename)
        {
            SystemPathUtils obj = new SystemPathUtils();
            TextReader tr = File.OpenText(obj.getProjectPath() + @"\Resources\API\Request\"+filename);
            string data = tr.ReadToEnd();
            tr.Close();
            return data;
        }

        public string UpdateRequestFromFile(string Data,string OldValue,string NewValue)
        {
            var UpdateData = Data.Replace(OldValue, NewValue);
            return UpdateData;
        }

     //   [Test]
        public void testJson()
        {
            JSONUtils obj = new JSONUtils();
            string s = obj.ReadRequestFromFile("SPQR_Request.json");
            using var doc = JsonDocument.Parse(s);
            JsonElement root = doc.RootElement;
            JsonElement dd = root.GetProperty("header").GetProperty("dataContractName");
            string req = UpdateRequestFromFile(s, "\"surveyRequired\": \"N\"", "\"surveyRequired\": \"Y\"");
            Console.WriteLine(UpdateRequestFromFile(req, "\"name\": \"APPOINTMENT_REQUIRED\"", "\"name\": \"APPOINTMENT_NOT_REQUIRED\""));


        }
    }
}

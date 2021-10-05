using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace UNITE.Utils
{
    class SystemPathUtils
    {
        public string getProjectPath()
        {
            string path = Directory.GetCurrentDirectory().Split("\\bin")[0];   
            return path+"\\";
        }

        // ConfigurationManager.AppSettings.Get("name1");
    }
}

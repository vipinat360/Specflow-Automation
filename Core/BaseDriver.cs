using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

/*
 * Created By: Chirag
*/
namespace UNITE.Core
{
    class BaseDriver : APIDriver
    {
        public RestClient apiinit()
        {
            RestClient client = new RestClient();
            return client;
        }
    }
}

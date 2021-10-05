using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

/*
 *Created By: Chirag
*/
namespace UNITE.Core
{
    interface APIDriver
    {
        RestClient apiinit();
    }

}

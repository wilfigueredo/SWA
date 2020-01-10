using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SWA.WF.Infra.Data.Util
{
    public class HelperApi
    {
        public HttpClient Cliente { get; set; }        
        public HelperApi(string uri,string type)
        {
            Cliente = new HttpClient();
            Cliente.BaseAddress = new Uri(uri);
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue($"application/{type}"));
        }
    }
}

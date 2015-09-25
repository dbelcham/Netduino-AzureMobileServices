using System;
using System.IO;
using System.Net;
using Json.NETMF;
using Microsoft.SPOT;

namespace Igloocoder.MF.AzureMobileServices
{
    public class MobileServiceEndpoint
    {
        private readonly string _applicationKey;
        private readonly Uri _uri;

        public MobileServiceEndpoint(Uri uri, string applicationKey)
        {
            _uri = uri;
            _applicationKey = applicationKey;
        }

        public HttpWebResponse Insert(string table, object data)
        {
            var json = JsonSerializer.SerializeObject(data);
            var request = (HttpWebRequest) WebRequest.Create(_uri.AbsoluteUri + "tables/" + table);
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = json.ToCharArray().Length;
            request.Headers.Add("X-ZUMO-APPLICATION", _applicationKey);
            
            Debug.Print("request configured");

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var response = (HttpWebResponse) request.GetResponse())
            {
                return response;
            }
        }
    }
}
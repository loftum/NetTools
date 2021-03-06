﻿using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Pat.Models
{
    public interface IInput
    {
        string Method { get; set; }
        string Url { get; set; }
        string CertFile { get; set; }
        string CertPass { get; set; }
        bool AllowAutoRedirect { get; set; }
        ObservableCollection<KeyValue<string, string>> Headers { get; set; }
        string Body { get; set; }
        HttpWebRequest CreateRequest();
    }

    public class Input : IInput
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string CertFile { get; set; }
        public string CertPass { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public string Body { get; set; }
        public ObservableCollection<KeyValue<string, string>> Headers { get; set; }
        
        public Input()
        {
            Headers = new ObservableCollection<KeyValue<string, string>>();
        }

        public HttpWebRequest CreateRequest()
        {
            var request = WebRequest.CreateHttp(Url);
            request.Method = Method;
            if (!string.IsNullOrWhiteSpace(CertFile))
            {
                request.ClientCertificates.Add(new X509Certificate2(File.ReadAllBytes(CertFile), CertPass));
            }
            
            request.AllowAutoRedirect = AllowAutoRedirect;
            foreach (var header in Headers)
            {
                var lowerKey = header.Key.ToLowerInvariant();
                switch (lowerKey)
                {
                    case "accept":
                        request.Accept = header.Value;
                        break;
                    case "user-agent":
                        request.UserAgent = header.Value;
                        break;
                    case "content-type":
                        request.ContentType = header.Value;
                        break;
                    case "referer":
                        request.Referer = header.Value;
                        break;
                    case "connection":
                        request.Connection = header.Value;
                        break;
                    case "expect":
                        request.Expect = header.Value;
                        break;
                    default:
                        request.Headers[header.Key] = header.Value;
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(Body))
            {
                using (var stream = request.GetRequestStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(Body);
                    }
                }
            }
            return request;
        }
    }
}
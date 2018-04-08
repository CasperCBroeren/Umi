﻿using System;
using System.Net;
using System.Net.Http;

namespace Umi.Core
{
    public class EndpointConfiguration
    {
        public Action<HttpRequestMessage> PreTest { get; set; }

        public Action<HttpResponseMessage, TestResult>  PostTest { get; set; }

        private string category = Default;
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = (value == null) ? Default : value;
            }
        }

        public HttpStatusCode TestAsSuccessStatusCode { get; set; }
        const string Default = "General";
    }
}

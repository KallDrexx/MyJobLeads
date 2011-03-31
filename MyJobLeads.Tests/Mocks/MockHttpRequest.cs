using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MyJobLeads.Tests.Mocks
{
    public class MockHttpRequest : HttpRequestBase
    {
        private readonly Uri _url = new Uri("http://mysite.example.com/");

        public override Uri Url
        {
            get
            {
                return _url;
            }
        }
    }
}

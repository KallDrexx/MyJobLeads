using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MyJobLeads.Tests.Mocks;

namespace MyJobLeads.Tests
{
    public class TestingBase
    {
        protected static void SetupController(Controller controller)
        {
            RequestContext requestContext = new RequestContext(new MockHttpContext(), new RouteData());
            controller.Url = new UrlHelper(requestContext);

            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                RequestContext = requestContext
            };
        }
    }
}

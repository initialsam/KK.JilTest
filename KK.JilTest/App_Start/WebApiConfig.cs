using KK.JilTest.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace KK.JilTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Replace Jil
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(new JilFormatter());

            // Insert方式
            //config.Formatters.RemoveAt(0);
            //config.Formatters.Insert(0, new JilFormatter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

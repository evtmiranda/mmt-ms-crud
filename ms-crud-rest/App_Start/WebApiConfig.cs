using ms_crud_rest.Exceptions;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ms_crud_rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //passa a aceitar somente json
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

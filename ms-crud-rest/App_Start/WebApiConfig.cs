using ms_crud_rest.Exceptions;
using System.Web.Http;

namespace ms_crud_rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Filtros de exception
            config.Filters.Add(new FilterHttpException());

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

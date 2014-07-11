using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using WebApiContrib.Tracing.Slab;

namespace WebApiGzip
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// http://benfoster.io/blog/aspnet-web-api-compression

			// https://github.com/azzlack/Microsoft.AspNet.WebApi.MessageHandlers.Compression
			// https://www.nuget.org/packages/Microsoft.AspNet.WebApi.MessageHandlers.Compression/
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));

			config.EnableSystemDiagnosticsTracing();
			config.Services.Replace(typeof(ITraceWriter), new SlabTraceWriter());

			config.Services.Add(typeof (IExceptionLogger), new SlabLoggingExceptionLogger());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Buffers;

namespace Web.Attributes
{
    public class JsonConfigFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var jsonOutputFormatter = new NewtonsoftJsonOutputFormatter(
                    serializerSettings,
                    ArrayPool<char>.Shared,
                    new MvcOptions { },
                    new MvcNewtonsoftJsonOptions()
                    );

                objectResult.Formatters.Add(jsonOutputFormatter);
            }

            base.OnResultExecuting(context);
        }
    }
}

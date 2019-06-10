using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesREST.Filters
{
    public class SchemaDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Applies filter context to swagger document.
        /// </summary>
        /// <param name="swaggerDoc"><see cref="SwaggerDocument"/> instance.</param>
        /// <param name="context"><see cref="DocumentFilterContext"/> instance.</param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Host = "nzmovierest.azurewebsites.net";
            swaggerDoc.BasePath = "/";
            swaggerDoc.Schemes = new List<string>() { "https" };
        }
    }
}
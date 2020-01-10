using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace SWA.WF.Services.Api.IO.Services.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "SWA Challenge API",
                    Description = "API SWA",
                    TermsOfService = "Nenhum",
                    Contact = new Contact { Name = "Williame", Email = "williamfigueredo@outlook.com", Url = "" },
                    License = new License { Name = "VB", Url = "" }
                });

                s.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }
    }
}
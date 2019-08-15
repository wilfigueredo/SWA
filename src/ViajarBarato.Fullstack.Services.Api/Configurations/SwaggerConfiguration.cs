using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ViajarBarato.Fullstack.Services.Api.IO.Services.Api.Configurations
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
                    Title = "Viajar Barato API",
                    Description = "API Desafio Challenge",
                    TermsOfService = "Nenhum",
                    Contact = new Contact { Name = "Williame", Email = "williamfigueredo@outlook.com", Url = "" },
                    License = new License { Name = "VB", Url = "" }
                });

                s.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }
    }
}
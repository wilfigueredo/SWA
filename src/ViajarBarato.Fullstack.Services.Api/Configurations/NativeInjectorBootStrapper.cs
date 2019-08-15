using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Services;
using ViajarBarato.Fullstack.Infra.Data.ExternalServices;
using ViajarBarato.Fullstack.Services.Data;

namespace ViajarBarato.Fullstack.Services.Api.Configurations
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();            
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IPersonagemService, PersonagemService>();
            services.AddScoped<IPersonagemRepositorio, PersonagemRepositorio>();
            services.AddScoped<IEspecieService, EspecieService>();
            services.AddScoped<IEspecieRepositorio, EspecieRepositorio>();
            services.AddScoped<IPlanetaRepositorio, PlanetaRepositorio>();

        }
    }
}

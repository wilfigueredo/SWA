using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWA.WF.Domain.Interface;
using SWA.WF.Domain.Services;
using SWA.WF.Infra.Data.ExternalServices;
using SWA.WF.Services.Data;

namespace SWA.WF.Services.Api.Configurations
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

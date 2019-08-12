using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Models;
using ViajarBarato.Fullstack.Infra.Data.DTO;
using ViajarBarato.Fullstack.Infra.Data.Util;

namespace ViajarBarato.Fullstack.Infra.Data.ExternalServices
{
    public class PlanetaRepositorio : IPlanetaRepositorio
    {
        public async Task<string> ObterHomeWorld(string homeworld)
        {
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync(homeworld);

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();

                var planeta = JsonConvert.DeserializeObject<PlanetaDTO>(dados);

                return planeta.name;
            }

            return "";
        }
    }
}

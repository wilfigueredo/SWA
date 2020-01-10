using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SWA.WF.Domain.Interface;
using SWA.WF.Domain.Models;
using SWA.WF.Infra.Data.DTO;
using SWA.WF.Infra.Data.Util;

namespace SWA.WF.Infra.Data.ExternalServices
{
    public class EspecieRepositorio : IEspecieRepositorio
    {
        public EspecieRepositorio(IPlanetaRepositorio planetaRepositorio)
        {
            especies = new List<EspecieDTO>();
            _planeta = planetaRepositorio;
        }

        EntityResults<EspecieDTO> swapiResponse;
        int page = 1;
        List<EspecieDTO> especies;
        private readonly IPlanetaRepositorio _planeta;

        public async Task<IEnumerable<Especie>> ObterTodos()
        {            
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync($"species/?page={page}");
            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();
                swapiResponse = JsonConvert.DeserializeObject<EntityResults<EspecieDTO>>(dados);

                await AdicionaResponseAListaAsync();                
                
                while (TemProximaPagina(swapiResponse))
                {                    
                    page++;
                    await ObterTodos();
                }

                return Mapper(especies);
            }

            return new List<Especie>();
        }        

        private async Task AdicionaResponseAListaAsync()
        {
            foreach (var item in swapiResponse.results)
            {
                item.HomeWorldName = new PlanetaDTO { name = await _planeta.ObterHomeWorld(item.homeworld) };
                especies.Add(item);
            }
        }

        private bool TemProximaPagina(EntityResults<EspecieDTO> swapiResponse)
        {
            return swapiResponse.next != null;
        }

        private IEnumerable<Especie> Mapper(List<EspecieDTO> especiesDTO)
        {
            List<Especie> especies = new List<Especie>();

            foreach (var item in especiesDTO)
            {
                Especie especie = new Especie();
                especie.Nome = item.name;
                especie.PlanetaNatal = new Planeta { Nome = item.HomeWorldName.name };
                especies.Add(especie);
            }
            return especies;
        }

        public async Task<string> ObterPorId(string url)
        {
            HelperApi api = new HelperApi(url, "json");
            HttpResponseMessage response = await api.Cliente.GetAsync("");

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();

                var especie = JsonConvert.DeserializeObject<EspecieDTO>(dados);

                return especie.name;
            }

            return "";
        }
    }
}


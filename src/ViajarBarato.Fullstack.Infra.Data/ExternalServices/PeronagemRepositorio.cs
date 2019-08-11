using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class PeronagemRepositorio : IPersonagem
    {
        public int Count { get; set; }
        public async Task<IEnumerable<KeyValuePair<Personagem,int>>> ObterPaginado(int page = 1)
        {
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync($"people?page={page}");

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();

                EntityResults<PersonagemDTO> swapiResponse = JsonConvert.DeserializeObject<EntityResults<PersonagemDTO>>(dados);
                
                List<PersonagemDTO> personagemCompleto = new List<PersonagemDTO>();

                foreach (var item in swapiResponse.results)
                {
                    item.HomeWorldName = await ObterPlanetaPorId(item.homeworld);
                    personagemCompleto.Add(item);
                }
                return PersonagemMapeado(personagemCompleto, Convert.ToInt32(swapiResponse.count));
            }

            return new List<KeyValuePair<Personagem,int>>();
        }

        private IEnumerable<KeyValuePair<Personagem, int>> PersonagemMapeado(List<PersonagemDTO> personagemCompleto,int count)
        {
            List<KeyValuePair<Personagem, int>> personagems = new List<KeyValuePair<Personagem, int>>();

            foreach (var item in personagemCompleto)
            {
                Personagem personagem = new Personagem();
                personagem.Nome = item.name;
                personagem.PlanetaNatal = new Planeta { Nome = item.HomeWorldName.name };
                personagems.Add(new KeyValuePair<Personagem, int>(personagem,count));
            }

            return personagems;
        }

        private async Task<PlanetaDTO> ObterPlanetaPorId(string homeworld)
        {
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync(homeworld);

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();

                var planeta = JsonConvert.DeserializeObject<PlanetaDTO>(dados);

                return planeta;
            }

            return new PlanetaDTO();
        }
    }
}

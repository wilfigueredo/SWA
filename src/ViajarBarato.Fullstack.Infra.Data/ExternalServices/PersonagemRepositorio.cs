using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Models;
using ViajarBarato.Fullstack.Infra.Data.DTO;
using ViajarBarato.Fullstack.Infra.Data.Util;

namespace ViajarBarato.Fullstack.Infra.Data.ExternalServices
{
    public class PersonagemRepositorio : IPersonagemRepositorio
    {
        public PersonagemRepositorio(IPlanetaRepositorio planetaRepositorio,
                                     IEspecieRepositorio especieRepositorio)
        {
            _planeta = planetaRepositorio;
            _especie = especieRepositorio;
            personagemCompleto = new List<PersonagemDTO>();
        }

        private readonly IPlanetaRepositorio _planeta;
        private readonly IEspecieRepositorio _especie;
        List<PersonagemDTO> personagemCompleto;
        public int page { get; set; } = 1;

        public async Task<IEnumerable<KeyValuePair<Personagem,int>>> ObterPaginado(int page = 1)
        {
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync($"people?page={page}");

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();

                EntityResults<PersonagemDTO> swapiResponse = JsonConvert.DeserializeObject<EntityResults<PersonagemDTO>>(dados);

                await AdicionaResponseAListaAsync(swapiResponse.results);
                
                return Mapper(personagemCompleto, Convert.ToInt32(swapiResponse.count));
            }

            return new List<KeyValuePair<Personagem,int>>();
        }

        private async Task AdicionaResponseAListaAsync(List<PersonagemDTO> results)
        {
            foreach (var item in results)
            {
                item.HomeWorldName = new PlanetaDTO { name = await _planeta.ObterHomeWorld(item.homeworld) };
                item.Especie = item.species.Count() > 0 ? new EspecieDTO { name = await _especie.ObterPorId(item.species[0]) } : new EspecieDTO { name = "" };
                personagemCompleto.Add(item);
            }
        }

        private IEnumerable<KeyValuePair<Personagem, int>> Mapper(List<PersonagemDTO> personagemCompleto,int count)
        {
            List<KeyValuePair<Personagem, int>> personagems = new List<KeyValuePair<Personagem, int>>();

            foreach (var item in personagemCompleto)
            {
                Personagem personagem = new Personagem();
                personagem.Nome = item.name;
                personagem.PlanetaNatal = new Planeta { Nome = item.HomeWorldName.name };
                personagem.Especie = new Especie { Nome = item.Especie.name };
                personagems.Add(new KeyValuePair<Personagem, int>(personagem,count));
            }

            return personagems;
        }

        public async Task<IEnumerable<Personagem>> ObterTodos()
        {
            List<Personagem> personagens = new List<Personagem>();
            var personagem = await ObterPaginado(page);
            personagens.AddRange(AdicionaPersonagemALista(personagem));
            var totalPage = Math.Ceiling(personagem.ElementAt(0).Value / Convert.ToDecimal(personagem.Count()));
            for(int i = page + 1; i <= totalPage; i++)
            {
                personagemCompleto.Clear();
                personagens.AddRange(AdicionaPersonagemALista(await ObterPaginado(i)));                
            }
            return personagens;
        }

        private List<Personagem> AdicionaPersonagemALista(IEnumerable<KeyValuePair<Personagem, int>> personagem)
        {
            List<Personagem> personagens = new List<Personagem>();
            foreach (var item in personagem)
            {
                personagens.Add(item.Key);
            }
            return personagens;
        }
    }
}

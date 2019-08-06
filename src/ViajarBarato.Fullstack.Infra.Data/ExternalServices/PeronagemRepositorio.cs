using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Models;
using ViajarBarato.Fullstack.Infra.Data.Util;

namespace ViajarBarato.Fullstack.Infra.Data.ExternalServices
{
    public class PeronagemRepositorio : IPersonagem
    {
        public async Task<IEnumerable<Personagem>> ObterPaginado(int page = 1)
        {
            HelperApi api = new HelperApi("https://swapi.co/api/", "json");
            HttpResponseMessage response = await api.Cliente.GetAsync($"people?page={page}");

            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();
               
                List<Personagem> personagem =  JsonConvert.DeserializeObject<List<Personagem>>(dados);

                foreach (var item in personagem)
                {
                    item.PlanetaNatal = ObterPlanetaPorId(item.PlanetaId);
                    item.EspecieId = ObterEspeciePorId(item.EspecieId);
                }
            }

            return new List<Personagem>();
        }

        private string ObterEspeciePorId(string especieId)
        {
            throw new NotImplementedException();
        }

        private Planeta ObterPlanetaPorId(string id)
        {
            throw new NotImplementedException();
        }
    }
}

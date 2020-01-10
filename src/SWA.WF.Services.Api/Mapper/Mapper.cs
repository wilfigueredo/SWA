using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWA.WF.Domain.Models;
using SWA.WF.Web.ViewModel;

namespace SWA.WF.Services.Api.Mapper
{
    public static class Mapper
    {
        public static IEnumerable<SwaViewModel> Map(IEnumerable<KeyValuePair<Personagem, int>> personagem)
        {
            List<SwaViewModel> personagens = new List<SwaViewModel>();

            foreach (var item in personagem)
            {
                SwaViewModel swa = new SwaViewModel();
                swa.NomePersonagem = item.Key.Nome;
                swa.PlanetaNatal = item.Key.PlanetaNatal.Nome;
                swa.count = item.Value;
                personagens.Add(swa);
            }
            return personagens;
        }

        public static IEnumerable<string> Map(IEnumerable<Especie> especies)
        {
            List<string> especiesViewModel = new List<string>();
            foreach (var item in especies)
            {
                especiesViewModel.Add(item.Nome + " (" + item.PlanetaNatal.Nome + ")");
            }
            return especiesViewModel;
        }

        public static IEnumerable<SwaViewModel> Map(IEnumerable<Personagem> personagens)
        {
            List<SwaViewModel> personagensViewModel = new List<SwaViewModel>();
            foreach (var item in personagens)
            {
                SwaViewModel swa = new SwaViewModel();
                swa.NomePersonagem = item.Nome;
                swa.PlanetaNatal = item.PlanetaNatal.Nome;
                swa.count = personagens.Count();
                personagensViewModel.Add(swa);
            }
            return personagensViewModel;
        }
    }
}

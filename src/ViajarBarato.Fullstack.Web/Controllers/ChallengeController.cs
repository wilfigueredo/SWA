using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Models;
using ViajarBarato.Fullstack.Infra.Data.ExternalServices;
using ViajarBarato.FullStack.Web.ViewModel;

namespace ViajarBarato.Fullstack.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly ILogger<ChallengeController> _logger;       
        private readonly IPersonagemService _personagemService;
        private readonly IEspecieService _especieService;

        public ChallengeController(ILogger<ChallengeController> logger,
            IPersonagemService personagemService,
            IEspecieService especieService)
        {
            _logger = logger;
            _personagemService = personagemService;
            _especieService = especieService;
        }

        [HttpGet]
        public async Task<IEnumerable<SwaViewModel>> Get(int page = 1)
        {                  
            var personagem = await _personagemService.ObterPaginado(page);
            return Mapper(personagem);
        }
        private IEnumerable<SwaViewModel> Mapper(IEnumerable<KeyValuePair<Personagem, int>> personagem)
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

        [HttpGet]
        [Route("GetEspecies")]
        public async Task<IEnumerable<string>> GetEspecies()
        {
            var especies = await _especieService.ObterTodos();
            return Mapper(especies);
        }

        private IEnumerable<string> Mapper(IEnumerable<Especie> especies)
        {
            List<string> especiesViewModel = new List<string>();
            foreach (var item in especies)
            {
                especiesViewModel.Add(item.Nome + " (" + item.PlanetaNatal.Nome + ")");
            }
            return especiesViewModel;
        }

        [HttpGet]
        [Route("FindByEspecies")]
        public async Task<IEnumerable<SwaViewModel>> FindByEspecies(string filter = "")
        {
            if (!(filter == "undefined")) {
                dynamic personagemFilter;                
                int index = filter.Trim().IndexOf("(") - 1;
                var personagens = await _personagemService.ObterTodos();
                if(index > 0)
                     personagemFilter = personagens.Where(p => p.Especie.Nome.Trim().ToLower() == filter.Trim().ToLower().Substring(0,index));
                else
                    personagemFilter = personagens.Where(p => p.Especie.Nome.Trim().ToLower().Contains(filter.Trim().ToLower()));

                return Mapper(personagemFilter);
            }
            else
            {                
                var personagem = await _personagemService.ObterPaginado(1);
                return Mapper(personagem);
            }
        }

        private IEnumerable<SwaViewModel> Mapper(IEnumerable<Personagem> personagens)
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
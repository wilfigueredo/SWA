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
        private IPersonagem _personagem;

        public ChallengeController(ILogger<ChallengeController> logger)
        {
            _logger = logger;           
        }

        [HttpGet]
        public async Task<IEnumerable<SwaViewModel>> Get(int page = 1)
        {
            _personagem = new PeronagemRepositorio();
            var personagem = await _personagem.ObterPaginado(page);
            return Mapper(personagem);
        }
        private IEnumerable<SwaViewModel> Mapper(IEnumerable<KeyValuePair<Personagem, int>> personagem)
        {
            List<SwaViewModel> personagems = new List<SwaViewModel>();            

            foreach (var item in personagem)
            {
                SwaViewModel swa = new SwaViewModel();
                swa.NomePersonagem = item.Key.Nome;
                swa.PlanetaNatal = item.Key.PlanetaNatal.Nome;
                swa.count = item.Value;
                personagems.Add(swa);
            }
            return personagems;
        }
    }
}
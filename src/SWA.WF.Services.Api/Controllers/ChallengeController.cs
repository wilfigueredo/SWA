using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SWA.WF.Domain.Interface;
using SWA.WF.Domain.Models;
using SWA.WF.Services.Api.Mapper;
using SWA.WF.Services.Api.Authorization;
using SWA.WF.Services.Api.Controllers;
using SWA.WF.Services.Api.Models;
using SWA.WF.Web.ViewModel;

namespace SWA.WF.Web.Controllers
{
    
    public class ChallengeController : BaseController
    {            
        private readonly IPersonagemService _personagemService;
        private readonly IEspecieService _especieService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly TokenDescriptor _tokenDescriptor;

        public ChallengeController(ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPersonagemService personagemService,
            IEspecieService especieService,
            TokenDescriptor tokenDescriptor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _personagemService = personagemService;
            _especieService = especieService;
            _tokenDescriptor = tokenDescriptor;
        }
        /// <summary>
        /// Obtem personagens e seu planeta natal por página
        /// </summary>
        /// <param name="page">pagina que deve buscar</param>
        /// <returns>SwaViewModel com todas insformações sobre o personagem e paginação</returns>
        [Authorize(Policy = "PodeLerPersonagem")]
        [HttpGet]
        [Route("challenge/{page:int}")]
        public async Task<IEnumerable<SwaViewModel>> Get(int page = 1)
        {                  
            var personagem = await _personagemService.ObterPaginado(page);
            return Mapper.Map(personagem);
        }
        
        /// <summary>
        /// Obtem todas as especies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "PodeLerPersonagem")]
        [Route("challenge/getEspecies")]
        public async Task<IEnumerable<string>> GetEspecies()
        {
            var especies = await _especieService.ObterTodos();
            return Mapper.Map(especies);
        }
        
        /// <summary>
        /// Busca as especiés por filtro
        /// </summary>
        /// <param name="cache">Instancia do redis cache</param>
        /// <param name="filter">Filtro que deve ser buscado</param>
        /// <returns>Retorna uma IEnumerable de swaViewModel para renderizar o grid filtrado</returns>
        [HttpGet]
        [Authorize(Policy = "PodeLerEspecie")]
        [Route("challenge/findByEspecies/{filter}")]
        public async Task<IEnumerable<SwaViewModel>> FindByEspecies([FromServices]IDistributedCache cache,
                                                                    string filter)
        {
            dynamic personagemFilter;
            IEnumerable<Personagem> personagens;
            int index = filter.Trim().IndexOf("(") - 1;

            string hasValue = cache.GetString("Personagens");

            personagens = await ObtemPersonagensAsync(hasValue, cache);

            if (index > 0)
                personagemFilter = personagens.Where(p => p.Especie.Nome.Trim().ToLower() == filter.Trim().ToLower().Substring(0, index));
            else
                personagemFilter = personagens.Where(p => p.Especie.Nome.Trim().ToLower().Contains(filter.Trim().ToLower()));

            DistributedCacheEntryOptions opcoesCache =
               new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(
                TimeSpan.FromMinutes(60));
            cache.SetString("Personagens", JsonConvert.SerializeObject(personagens), opcoesCache);

            return Mapper.Map(personagemFilter);

        }

        /// <summary>
        /// Verifica se obtem do banco ou do cache
        /// </summary>
        /// <param name="hasValue">Verifica se existe valor no cache</param>
        /// <param name="cache">instancia do redis</param>
        /// <returns></returns>
        private async Task<IEnumerable<Personagem>> ObtemPersonagensAsync(string hasValue, IDistributedCache cache)
        {
            IEnumerable<Personagem> personagens;

            if (hasValue == null)
            {
                return personagens = await _personagemService.ObterTodos();
            }
            else
            {
                return personagens = JsonConvert.DeserializeObject<IEnumerable<Personagem>>(cache.GetString("Personagens"));
            }            
        }        
    }
}
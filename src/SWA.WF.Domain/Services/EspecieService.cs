using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SWA.WF.Domain.Interface;
using SWA.WF.Domain.Models;

namespace SWA.WF.Domain.Services
{
    public class EspecieService : IEspecieService
    {
        private readonly IEspecieRepositorio _especieRepositorio;

        public EspecieService(IEspecieRepositorio especieRepositorio)
        {
            _especieRepositorio = especieRepositorio;
        }

        public Task<IEnumerable<Especie>> ObterTodos()
        {
            return _especieRepositorio.ObterTodos();
        }
    }
}

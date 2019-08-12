using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Interface;
using ViajarBarato.Fullstack.Domain.Models;

namespace ViajarBarato.Fullstack.Domain.Services
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

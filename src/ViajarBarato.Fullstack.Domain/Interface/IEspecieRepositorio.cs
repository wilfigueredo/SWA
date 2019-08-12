using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViajarBarato.Fullstack.Domain.Models;

namespace ViajarBarato.Fullstack.Domain.Interface
{
    public interface IEspecieRepositorio
    {
        Task<IEnumerable<Especie>> ObterTodos();
        Task<string> ObterPorId(string url);
    }
}

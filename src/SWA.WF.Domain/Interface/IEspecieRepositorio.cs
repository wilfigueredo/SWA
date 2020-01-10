using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SWA.WF.Domain.Models;

namespace SWA.WF.Domain.Interface
{
    public interface IEspecieRepositorio
    {
        Task<IEnumerable<Especie>> ObterTodos();
        Task<string> ObterPorId(string url);
    }
}

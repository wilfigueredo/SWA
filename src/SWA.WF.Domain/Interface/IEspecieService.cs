using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SWA.WF.Domain.Models;

namespace SWA.WF.Domain.Interface
{
    public interface IEspecieService
    {
        Task<IEnumerable<Especie>> ObterTodos();        
    }
}

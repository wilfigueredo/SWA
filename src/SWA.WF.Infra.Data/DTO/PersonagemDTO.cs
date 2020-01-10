using System.Collections.Generic;
using System.Text;
using SWA.WF.Infra.Data.Interface;

namespace SWA.WF.Infra.Data.DTO
{

    public class PersonagemDTO : IEntityApi
    {              
        public string name { get; set; }       
        public string homeworld { get; set; }
        public List<string> species { get; set; }
        public PlanetaDTO HomeWorldName { get; set; }
        public EspecieDTO Especie { get; set; }
    }
}

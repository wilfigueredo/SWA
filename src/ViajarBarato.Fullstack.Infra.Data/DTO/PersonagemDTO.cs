using System.Collections.Generic;
using System.Text;
using ViajarBarato.Fullstack.Infra.Data.Interface;

namespace ViajarBarato.Fullstack.Infra.Data.DTO
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

using System;
using System.Collections.Generic;
using System.Text;

namespace ViajarBarato.Fullstack.Domain.Models
{
    public class Personagem
    {
        public string Nome { get; set; }        
        public Planeta PlanetaNatal { get; set; }
        public Especie Especie { get; set; }
    }
}

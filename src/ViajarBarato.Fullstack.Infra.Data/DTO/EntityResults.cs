using System;
using System.Collections.Generic;
using ViajarBarato.Fullstack.Infra.Data.Interface;

namespace ViajarBarato.Fullstack.Infra.Data.DTO
{
    public class EntityResults<T> : IEntityApi where T : IEntityApi
    {      
        public Int64 count { get; set; }
        public string next { get; set; }
        public List<T> results { get; set; }
    }
}

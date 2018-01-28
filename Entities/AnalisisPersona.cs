using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class AnalisisPersona
    {
        public int Id { get; set; }
        public string RutPersona { get; set; }
        public string IdAnalisis { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class AccionInmediataPersona
    {
        public int Id { get; set; }
        public string RutPersona { get; set; }
        public string IdEvento { get; set; }
        public string IdAccionInmediata { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
    }
}

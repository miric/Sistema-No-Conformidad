using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class AccionInmediata
    {
        public string Id { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Estado { get; set; }
        public bool? Efectivo { get; set; }
        public string EventoId { get; set; }
        public string RutPersona { get; set; }
        public string Descripcion { get; set; }
        public bool Removed { get; set; }

        public virtual Evento Evento { get; set; }
    }
}

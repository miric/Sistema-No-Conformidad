using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class AccionCorrectiva
    {
        public string Id { get; set; }
        public string EventoId { get; set; }
        public string Causa { get; set; }
        public string AccionCorrectiva1 { get; set; }
        public string Descripcion { get; set; }
        public string RutPersona { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public DateTime? FechaLimite { get; set; }
        public bool? Efectivo { get; set; }
        public string Estado { get; set; }
        public bool Removed { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Persona RutPersonaNavigation { get; set; }
    }
}

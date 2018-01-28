using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class PersonaEventoInvolucrado
    {
        public int Id { get; set; }
        public string RutPersona { get; set; }
        public string IdEvento { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
        public int? Estado { get; set; }

        public virtual Evento IdEventoNavigation { get; set; }
        public virtual Persona RutPersonaNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class PersonaArea
    {
        public int Id { get; set; }
        public string RutPersona { get; set; }
        public int? IdArea { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }

        public virtual Area IdAreaNavigation { get; set; }
        public virtual Persona RutPersonaNavigation { get; set; }
    }
}

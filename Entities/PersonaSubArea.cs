using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class PersonaSubArea
    {
        public int Id { get; set; }
        public string RutPersona { get; set; }
        public int? IdSubArea { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }

        public virtual SubArea IdSubAreaNavigation { get; set; }
        public virtual Persona RutPersonaNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class SubArea
    {
        public SubArea()
        {
            Evento = new HashSet<Evento>();
            PersonaSubArea = new HashSet<PersonaSubArea>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? IdArea { get; set; }
        public bool? Removed { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
        public virtual ICollection<PersonaSubArea> PersonaSubArea { get; set; }
    }
}

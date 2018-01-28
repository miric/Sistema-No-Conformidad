using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class TipoEvento
    {
        public TipoEvento()
        {
            Evento = new HashSet<Evento>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public bool? Removed { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
    }
}

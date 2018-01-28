using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Estado
    {
        public Estado()
        {
            Evento = new HashSet<Evento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Removed { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class OrigenFalla
    {
        public OrigenFalla()
        {
            AnalisisCausaRaiz = new HashSet<AnalisisCausaRaiz>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Removed { get; set; }

        public virtual ICollection<AnalisisCausaRaiz> AnalisisCausaRaiz { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class FallaSecundaria
    {
        public FallaSecundaria()
        {
            AnalisisCausaRaiz = new HashSet<AnalisisCausaRaiz>();
        }

        public int Id { get; set; }
        public int IdFallaPrimaria { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<AnalisisCausaRaiz> AnalisisCausaRaiz { get; set; }
    }
}

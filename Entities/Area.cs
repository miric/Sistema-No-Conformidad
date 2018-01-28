using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Area
    {
        public Area()
        {
            PersonaArea = new HashSet<PersonaArea>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutJefe { get; set; }
        public bool? Removed { get; set; }

        public virtual ICollection<PersonaArea> PersonaArea { get; set; }
    }
}

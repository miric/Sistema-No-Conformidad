using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Verificacion
    {
        public string Id { get; set; }
        public string IdEvento { get; set; }
        public bool? Efectiva { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string RutPersona { get; set; }
        public string Estado { get; set; }
        public bool Removed { get; set; }

        public virtual Evento IdEventoNavigation { get; set; }
    }
}

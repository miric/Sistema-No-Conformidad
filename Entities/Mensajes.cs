using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Mensajes
    {
        public int Id { get; set; }
        public string IdEvento { get; set; }
        public string RutPersona { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public bool Removed { get; set; }
    }
}

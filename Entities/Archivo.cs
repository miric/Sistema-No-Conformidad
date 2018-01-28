using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Archivo
    {
        public int FileId { get; set; }
        public string Identificador { get; set; }
        public string Identificador2 { get; set; }
        public string Nombre { get; set; }
        public string Persona { get; set; }
        public string Tipo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
    }
}

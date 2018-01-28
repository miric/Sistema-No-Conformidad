using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class RespuestaCliente
    {
        public string Id { get; set; }
        public string IdEvento { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
    }
}

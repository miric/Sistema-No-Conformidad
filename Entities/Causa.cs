using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Causa
    {
        public int Id { get; set; }
        public string IdRca { get; set; }
        public string Causa1 { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Removed { get; set; }
        public int? IdOrigenFalla { get; set; }
        public int? IdTipoFalla { get; set; }
    }
}

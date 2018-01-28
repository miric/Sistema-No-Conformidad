using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class TipoFalla
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Removed { get; set; }
    }
}

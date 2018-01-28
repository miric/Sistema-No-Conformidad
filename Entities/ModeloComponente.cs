using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class ModeloComponente
    {
        public int Id { get; set; }
        public int IdModelo { get; set; }
        public int IdComponente { get; set; }
        public bool? Removed { get; set; }
    }
}

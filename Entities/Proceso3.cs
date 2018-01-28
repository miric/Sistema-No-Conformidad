using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Proceso3
    {
        public int Id { get; set; }
        public int IdProceso2 { get; set; }
        public string Nombre { get; set; }
    }
}

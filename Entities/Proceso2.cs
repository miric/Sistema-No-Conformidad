using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Proceso2
    {
        public int Id { get; set; }
        public int IdProceso1 { get; set; }
        public string Nombre { get; set; }
    }
}

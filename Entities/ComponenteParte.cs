using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class ComponenteParte
    {
        public int Id { get; set; }
        public int IdComponente { get; set; }
        public int IdParte { get; set; }
        public bool? Removed { get; set; }
    }
}

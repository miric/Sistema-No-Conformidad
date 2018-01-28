﻿using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Equipo
    {
        public Equipo()
        {
            Evento = new HashSet<Evento>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string SerieEquipo { get; set; }
        public bool? Removed { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
    }
}

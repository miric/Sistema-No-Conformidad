using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class AnalisisCausaRaiz
    {
        public string Id { get; set; }
        public string IdEvento { get; set; }
        public int? IdTipoFalla { get; set; }
        public int? IdOrigenFalla { get; set; }
        public string RutPersona { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaLimite { get; set; }
        public string Descripcion { get; set; }
        public int? IdCausa1 { get; set; }
        public int? IdCausa2 { get; set; }
        public int? IdCausa3 { get; set; }
        public int? IdCausa4 { get; set; }
        public int? IdCausa5 { get; set; }
        public int? IdCausaIntermedia1 { get; set; }
        public string Estado { get; set; }
        public bool Removed { get; set; }
        public long? Costo { get; set; }
        public int? IdCausaIntermedia2 { get; set; }
        public int? IdProceso1 { get; set; }
        public int? IdProceso2 { get; set; }
        public int? IdProceso3 { get; set; }
        public int? IdProceso4 { get; set; }
        public int? IdProceso5 { get; set; }
        public int? IdFallaPrimaria { get; set; }
        public int? IdFallaSecundaria { get; set; }
        public string Creador { get; set; }

        public virtual Evento IdEventoNavigation { get; set; }
        public virtual FallaPrimaria IdFallaPrimariaNavigation { get; set; }
        public virtual FallaSecundaria IdFallaSecundariaNavigation { get; set; }
        public virtual OrigenFalla IdOrigenFallaNavigation { get; set; }
    }
}

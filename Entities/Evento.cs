using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Evento
    {
        public Evento()
        {
            AccionCorrectiva = new HashSet<AccionCorrectiva>();
            AccionInmediata = new HashSet<AccionInmediata>();
            Analisis = new HashSet<Analisis>();
            AnalisisCausaRaiz = new HashSet<AnalisisCausaRaiz>();
            Evaluacion = new HashSet<Evaluacion>();
            PersonaEvento = new HashSet<PersonaEvento>();
            PersonaEventoInvolucrado = new HashSet<PersonaEventoInvolucrado>();
            Verificacion = new HashSet<Verificacion>();
        }

        public string Id { get; set; }
        public string Woanterior { get; set; }
        public string WorkOrder { get; set; }
        public int? TipoEvento { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Categorizacion { get; set; }
        public string Descripcion { get; set; }
        public bool? Ncaceptada { get; set; }
        public string Creador { get; set; }
        public int? Cliente { get; set; }
        public int? Equipo { get; set; }
        public int? Modelo { get; set; }
        public string SerieEquipo { get; set; }
        public int? Componente { get; set; }
        public string PosicionComponente { get; set; }
        public int? Parte { get; set; }
        public string NumeroParteComponente { get; set; }
        public string SerieComponente { get; set; }
        public string TipoFalla { get; set; }
        public int? HorasComponente { get; set; }
        public bool Removed { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaIdentificacion { get; set; }
        public int? Area { get; set; }
        public int? SubArea { get; set; }
        public int? Impacto { get; set; }
        public int? Probabilidad { get; set; }
        public int? Consecuencia { get; set; }
        public string Responsable { get; set; }
        public string RutJefeInvolucrado { get; set; }

        public virtual ICollection<AccionCorrectiva> AccionCorrectiva { get; set; }
        public virtual ICollection<AccionInmediata> AccionInmediata { get; set; }
        public virtual ICollection<Analisis> Analisis { get; set; }
        public virtual ICollection<AnalisisCausaRaiz> AnalisisCausaRaiz { get; set; }
        public virtual ICollection<Evaluacion> Evaluacion { get; set; }
        public virtual ICollection<PersonaEvento> PersonaEvento { get; set; }
        public virtual ICollection<PersonaEventoInvolucrado> PersonaEventoInvolucrado { get; set; }
        public virtual ICollection<Verificacion> Verificacion { get; set; }
        public virtual Cliente ClienteNavigation { get; set; }
        public virtual Componente ComponenteNavigation { get; set; }
        public virtual Equipo EquipoNavigation { get; set; }
        public virtual Estado EstadoNavigation { get; set; }
        public virtual Modelo ModeloNavigation { get; set; }
        public virtual Parte ParteNavigation { get; set; }
        public virtual Persona RutJefeInvolucradoNavigation { get; set; }
        public virtual SubArea SubAreaNavigation { get; set; }
        public virtual TipoEvento TipoEventoNavigation { get; set; }
    }
}

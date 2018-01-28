using System;
using System.Collections.Generic;

namespace FINNINGWEB.Entities
{
    public partial class Persona
    {
        public Persona()
        {
            AccionCorrectiva = new HashSet<AccionCorrectiva>();
            Evento = new HashSet<Evento>();
            PersonaArea = new HashSet<PersonaArea>();
            PersonaEvento = new HashSet<PersonaEvento>();
            PersonaEventoInvolucrado = new HashSet<PersonaEventoInvolucrado>();
            PersonaSubArea = new HashSet<PersonaSubArea>();
        }

        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string RutSupervisor { get; set; }
        public int? IdArea { get; set; }
        public int? IdSubArea { get; set; }
        public bool? Removed { get; set; }
        public string Cargo { get; set; }
        public string RutJefe { get; set; }

        public virtual ICollection<AccionCorrectiva> AccionCorrectiva { get; set; }
        public virtual ICollection<Evento> Evento { get; set; }
        public virtual ICollection<PersonaArea> PersonaArea { get; set; }
        public virtual ICollection<PersonaEvento> PersonaEvento { get; set; }
        public virtual ICollection<PersonaEventoInvolucrado> PersonaEventoInvolucrado { get; set; }
        public virtual ICollection<PersonaSubArea> PersonaSubArea { get; set; }
    }
}

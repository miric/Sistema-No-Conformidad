using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FINNINGWEB.Entities;
using System.ComponentModel.DataAnnotations;

namespace FINNINGWEB.Models
{
    public class AccionInmediataModels
    {
        public string EventoId { get; set; }
        public string IdAccionInmediata { get; set; }
        [Required]
        public string RutPersona { get; set; }
        public string NombreCompleto { get; set; }
        [StringLength(500, ErrorMessage = "El campo Descripción excede los 500 caracteres permitidos.")]
        public string Descripcion { get; set; }
        public bool Efectiva { get; set; }
        public Persona PersonaUnica { get; set; }
        public AccionInmediata AccionUnica { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
    }
}

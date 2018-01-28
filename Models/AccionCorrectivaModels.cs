using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FINNINGWEB.Entities;
using System.ComponentModel.DataAnnotations;

namespace FINNINGWEB.Models
{
    public class AccionCorrectivaModels
    {
        public string EventoId { get; set; }
        public string IdAccionCorrectiva { get; set; }
        [Required]
        public string RutPersona { get; set; }
        public string NombreCompleto { get; set; }
        [StringLength(500, ErrorMessage = "El Campo Causa excede los 500 caracteres permitidos.")]
        public string Causa { get; set; }
        [StringLength(500, ErrorMessage = "El Campo Accion Correctiva excede los 500 caracteres permitidos.")]
        public string AccionCorrectiva { get; set; }
        [StringLength(500, ErrorMessage = "El Campo Descripción excede los 500 caracteres permitidos.")]
        public string Descripcion { get; set; }
        public DateTime FechaLImite { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public bool Efectiva { get; set; }
        public Persona PersonaUnica { get; set; }
        public AccionCorrectiva AccionUnica { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<Archivo> ListaArchivos { get; set; }
    }
}



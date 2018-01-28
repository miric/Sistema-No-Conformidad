using FINNINGWEB.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class VerificacionModels
    {
        public bool? efectivo { get; set; }
        public string EventoId { get; set; }
        [StringLength(500, ErrorMessage = "El Campo Descripción excede los 500 caracteres permitidos.")]
        public string Descripcion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string rutPersona { get; set; }
        public bool Removed { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public Verificacion VerificacionUnica { get; set; }
    }
}

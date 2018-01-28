using FINNINGWEB.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FINNINGWEB.Models
{
    public class AnalisisModels
    {

        public string EventoId { get; set; }
        public string IdAnalisis { get; set; }
        public DateTime? FechaRegistro { get; set; }
        [StringLength(500, ErrorMessage = "El campo Descripcion debe ser una cadena con una longitud máxima de 500 caracteres.")]
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public bool AplicaRCA { get; set; }
        public string rutPersona { get; set; }
        public bool Removed { get; set; }
        public Persona PersonaUnica { get; set; }
        public Analisis AnalisisUnico { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }


    }

    public class AnalisisCausaRaizModels
    {

        public string EventoId { get; set; }
        public string IdAnalisis { get; set; }
        public DateTime? FechaRegistro { get; set; }
        [StringLength(500, ErrorMessage = "El campo Descripción excede los 500 caracteres permitidos.")]
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public bool NCAceptada { get; set; }
        public string rutPersona { get; set; }
        public bool Removed { get; set; }
        [StringLength(500, ErrorMessage = "El campo Descripción excede los 500 caracteres permitidos.")]
        public string causa { get; set; }
        public int OrigenFalla { get; set; }
        public long? Costo { get; set; }
        public Evento EventoUnico { get; set; }
        public Causa CausaUnica { get; set; }
        public AnalisisCausaRaiz RCAUnica { get; set; }
        public Persona PersonaUnica { get; set; }
        public Analisis AnalisisUnico { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<PersonaEvento> ListaPersonaEvento { get; set; }
        public IEnumerable<Archivo> ListaArchivos { get; set; }
        public IEnumerable<OrigenFalla> ListaOrigenFalla { get; set; }
        public IEnumerable<Causa1> ListaCausa1 { get; set; }
        public IEnumerable<Causa2> ListaCausa2 { get; set; }
        public IEnumerable<Causa3> ListaCausa3 { get; set; }
        public IEnumerable<Causa4> ListaCausa4 { get; set; }
        public IEnumerable<Causa5> ListaCausa5 { get; set; }
        public IEnumerable<CausaIntermedia1> ListaCausaIntermedia1 { get; set; }
        public IEnumerable<CausaIntermedia2> ListaCausaIntermedia2 { get; set; }
        public int? Causa1 { get; set; }
        public int? Causa2 { get; set; }
        public int? Causa3 { get; set; }
        public int? Causa4 { get; set; }
        public int? Causa5 { get; set; }
        public int? CausaIntermedia1 { get; set; }
        public int? CausaIntermedia2 { get; set; }
        public IEnumerable<Proceso1> ListaProceso1 { get; set; }
        public IEnumerable<Proceso2> ListaProceso2 { get; set; }
        public IEnumerable<Proceso3> ListaProceso3 { get; set; }
        public IEnumerable<Proceso4> ListaProceso4 { get; set; }
        public IEnumerable<Proceso5> ListaProceso5 { get; set; }
        public int? Proceso1 { get; set; }
        public int? Proceso2 { get; set; }
        public int? Proceso3 { get; set; }
        public int? Proceso4 { get; set; }
        public int? Proceso5 { get; set; }
        public IEnumerable<FallaPrimaria> ListaFallaPrimaria { get; set; }
        public IEnumerable<FallaSecundaria> ListaFallaSecundaria { get; set; }
        public int? FallaPrimaria { get; set; }
        public int? FallaSecundaria { get; set; }



    }
}

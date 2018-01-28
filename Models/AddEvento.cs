using FINNINGWEB.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Models
{
    public class AddEvento
    {

        public int IdTipo { get; set; }
        public bool? DevuelveComponente { get; set; }
        public bool? InformeFalla { get; set; }
        public bool? AntesDespuesDespacho { get; set; }
        public TipoEvento Tipo { get; set; }
        [StringLength(50, ErrorMessage = "El campo Work Order Anterior debe ser una cadena con una longitud máxima de 50 caracteres.")]
        public string WorkOrderAnterior { get; set; }
        [StringLength(50, ErrorMessage = "El campo Work Order Nueva debe ser una cadena con una longitud máxima de 50 caracteres.")]
        public string WorkOrderNueva { get; set; }
        public int? EquipoId { get; set; }
        public int? ModeloId { get; set; }
        public int? ComponenteId { get; set; }
        public int? ParteId { get; set; }
        public int? ClienteId { get; set; }
        [StringLength(50, ErrorMessage = "El campo Serie Equipo debe ser una cadena con una longitud máxima de 50 caracteres.")]
        public string SerieEquipo { get; set; }
        [StringLength(50, ErrorMessage = "El campo Numero Parte Componente debe ser una cadena con una longitud máxima de 50 caracteres.")]
        public string NumeroParteComponente { get; set; }
        [StringLength(2000, ErrorMessage = "El campo descripcion debe ser una cadena con una longitud máxima de 2000 caracteres.")]
        public string Descripcion { get; set; }
        [Range(0, 999999999, ErrorMessage = "El campo Horas Componente debe ser un numero entre 0 y 999.999.999")]
        public int? HorasComponente { get; set; }
        [StringLength(50, ErrorMessage = "El campo Serie Componente debe ser una cadena con una longitud máxima de 50 caracteres.")]
        public string SerieComponente { get; set; }
        public DateTime? FechaIdentificacion { get; set; }
        public string EditDescripcion { get; set; }
        public AspNetUsers Usuario { get; set; }
        public string NombreUsuario { get; set; }
        public Persona personaCapturada { get; set; }
        public Persona personaUnica { get; set; }
        public IEnumerable<Equipo> equipo { get; set; }
        public IEnumerable<Evento> Evento { get; set; }
        public IEnumerable<TipoEvento> TipoEvento { get; set; }
        public IEnumerable<Cliente> Cliente { get; set; }
        public IEnumerable<Modelo> modelo { get; set; }
        public IEnumerable<Componente> ListaComponente { get; set; }
        public IEnumerable<Parte> ListaParte { get; set; }
        public Evento EventoUnico { get; set; }
        public PersonaEvento PersonaEventoUnico { get; set; }
        public IEnumerable<PersonaEvento> personaEvento { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<Componente> componenteJOINmodelo { get; set; }
        public IEnumerable<Parte> componenteJOINparte { get; set; }
        public IEnumerable<Area> ListaAreas { get; set; }
        public IEnumerable<SubArea> ListaSubAreas { get; set; }
        public SubArea SubAreaUnica { get; set; }
        public Area AreaUnica { get; set; }
        public int? IdArea { get; set; }
        public int? IdSubArea { get; set; }
        [Range(0, 100, ErrorMessage = "El campo probabilidad debe ser un numero entre 0 y 100")]
        public int? Probabilidad { get; set; }
        [Range(0, 100, ErrorMessage = "El campo Consecuencia debe ser un numero entre 0 y 100")]
        public int? Consecuencia { get; set; }


    }

    public class ListaEvento
    {
        public PaginatedList<FINNINGWEB.Entities.Evento> probando { get; set; }
        public Evento Evento { get; set; }
        public List<Evento> ListEvento { get; set; }
        public List<Estado> ListEstado { get; set; }
        public IEnumerable<Cliente> Cliente { get; set; }
        public IEnumerable<Equipo> Equipo { get; set; }
        public IEnumerable<Modelo> Modelo { get; set; }
        public IEnumerable<Componente> Componente { get; set; }
        public IEnumerable<Parte> Parte { get; set; }
        public IEnumerable<TipoEvento> TipoEvento { get; set; }
        public string search { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoEventoSearch { get; set; }
        public string AnioSearch { get; set; }
        public string AsignacionSearch { get; set; }
        public string AplicaRCASearch { get; set; }
        public string optradio { get; set; }
        public string idEventoSearch { get; set; }
        public int? criticidadSearch { get; set; }
        public int? DiasSearch { get; set; }
        public int? HorasSearch { get; set; }
        public string NoConformidadSearch { get; set; }
        public string MesSearch { get; set; }
        public int? EstadoSearch { get; set; }
        public DateTime? FechaActual { get; set; }
    }

    public class RespuestaClienteModel {
        public string EventoId { get; set; }
        [Required(ErrorMessage = "El campo Fecha es requerido.")]
        public DateTime? FechaRegistro { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido.")]
        [StringLength(2000, ErrorMessage = "El campo Descripción debe ser una cadena con una longitud máxima de 2000 caracteres.")]
        public string Descripcion { get; set; }
    }
}

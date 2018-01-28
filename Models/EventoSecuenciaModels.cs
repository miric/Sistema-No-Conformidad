using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FINNINGWEB.Entities;

namespace FINNINGWEB.Models
{
    public class EventoSecuenciaModels
    {
        string NombreArchivo { get; set; }
        public Evento EventoUnico { get; set; }
        public RespuestaCliente RespuestaUnica { get; set; }
        public string TipoEvento { get; set; }
        public string Cliente { get; set; }
        public string Equipo { get; set; }
        public string Modelo { get; set; }
        public string Componente { get; set; }
        public string parte { get; set; }
        public string Responsable { get; set; }
        public string Involucrado { get; set; }
        public string ResponsableInvolucrado { get; set; }
        public string ResponsableInvolucradoNombre { get; set; }
        public string descripcion { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string Creador { get; set; }
        public string CreadorRCA { get; set; }
        public int? OrigenFalla { get; set; }
        public IEnumerable<OrigenFalla> ListaOrigenFalla { get; set; }
        public Analisis analisis { get; set; }
        public Evaluacion Evaluacion { get; set; }
        public Verificacion Verificacion { get; set; }
        public AccionInmediata AI { get; set; }
        public IEnumerable<AccionInmediata> ListaAccionesInmediatas { get; set; }
        public IEnumerable<AccionCorrectiva> ListaAccionCorrectiva { get; set; }
        public AnalisisCausaRaiz RCA { get; set; }
        public IEnumerable<Causa> ListaCausas { get; set; }
        public IEnumerable<Persona> ListaPersonas { get; set; }
        public IEnumerable<Persona> ListaPersonasAccionInmediata { get; set; }
        public IEnumerable<Persona> ListaPersonasAccionCorrectiva { get; set; }
        public IEnumerable<Persona> ListaSupervisores { get; set; }
        public IEnumerable<Persona> ListaPersonasResponsable { get; set; }
        public IEnumerable<Persona> ListaPersonasInvolucrados { get; set; }
        public Persona PersonaUnica { get; set; }
        public IEnumerable<Archivo> ListaArchivos { get; set; }
        public IEnumerable<PersonaEvento> ListaPersonaEvento { get; set; }
        public IEnumerable<PersonaEventoInvolucrado> ListaPersonaEventoInvolucrado { get; set; }
        public IEnumerable<Persona> ListaInvolucradosAccionInmediata { get; set; }
        public IEnumerable<Mensajes> ListaMensajes { get; set; }
        public IEnumerable<Estado> ListaEstado { get; set; }
        public IEnumerable<Area> ListaArea { get; set; }
        public IEnumerable<SubArea> ListaSubArea { get; set; }
        public int IdMensaje { get; set; }
        public int IdEliminarCausa { get; set; }
        public int IdEvento { get; set; }
        public int? Causa1 { get; set; }
        public int? Causa2 { get; set; }
        public int? Causa3 { get; set; }
        public int? Causa4 { get; set; }
        public int? Causa5 { get; set; }
        public int? CausaIntermedia1 { get; set; }
        public int? CausaIntermedia2 { get; set; }
        public IEnumerable<Causa1> ListaCausa1 { get; set; }
        public IEnumerable<Causa2> ListaCausa2 { get; set; }
        public IEnumerable<Causa3> ListaCausa3 { get; set; }
        public IEnumerable<Causa4> ListaCausa4 { get; set; }
        public IEnumerable<Causa5> ListaCausa5 { get; set; }
        public IEnumerable<CausaIntermedia1> ListaCausaIntermedia1 { get; set; }
        public IEnumerable<CausaIntermedia2> ListaCausaIntermedia2 { get; set; }
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


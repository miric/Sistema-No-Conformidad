using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;
using FINNINGWEB.Models;
using Microsoft.AspNetCore.Authorization;
using FINNINGWEB.Entities;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize]
    public class EventoSecuenciaController : Controller
    {
        private IHostingEnvironment _enviroment;
        private UserManager<AppUser> userManager;
        public EventoSecuenciaController(UserManager<AppUser> userMgr, IHostingEnvironment enviroment)
        {
            userManager = userMgr;
            _enviroment = enviroment;
        }
        private AdventureWorksContext db = new AdventureWorksContext();
        // GET: /<controller>/


        public IActionResult Index(string EventoID)
        {

            string CapturandoUsuario = null;
            ClaimsPrincipal currentUser = User;
            PruebaUser prueba;

            Evento EventoVariable = db.Evento.FirstOrDefault(c => c.Id == EventoID);
            int? Causa01 = null;
            int? Causa02= null;
            int? Causa03= null;
            int? Causa04= null;
            int? Causa05= null;
            int? CausaIntermedia01=null;
            int? CausaIntermedia02 = null;
            int? OrigenFalla0 = null;
            int? ParteInt = null;
            int? FallaPrimaria01 = null;
            int? FallaSecundaria01 = null;

            if (EventoVariable.Ncaceptada==true)
            {
                if (db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == EventoID && !c.Removed == true)!=null)
                {
                    AnalisisCausaRaiz acr = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == EventoID && !c.Removed == true);

                    if (db.OrigenFalla.FirstOrDefault(c => c.Id == acr.IdOrigenFalla) == null)
                    {
                        OrigenFalla0 = null;
                    }
                    else
                    {
                        OrigenFalla0 = db.OrigenFalla.FirstOrDefault(c => c.Id == acr.IdOrigenFalla).Id;
                    }
                    if (db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria) == null)
                    {
                        FallaPrimaria01 = null;
                    }
                    else
                    {
                        FallaPrimaria01 = db.FallaPrimaria.FirstOrDefault(c => c.Id == acr.IdFallaPrimaria).Id;
                    }
                    if (db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria) == null)
                    {
                        FallaSecundaria01 = null;
                    }
                    else
                    {
                        FallaSecundaria01 = db.FallaSecundaria.FirstOrDefault(c => c.Id == acr.IdFallaSecundaria).Id;
                    }

                    if (db.Causa1.FirstOrDefault(c => c.Id == acr.IdCausa1) == null)
                    {
                        Causa01 = null;
                    }
                    else
                    {
                        Causa01 = db.Causa1.FirstOrDefault(c => c.Id == acr.IdCausa1).Id;
                    }
                    if (db.Causa2.FirstOrDefault(c => c.Id == acr.IdCausa2) == null)
                    {
                        Causa02 = null;
                    }
                    else
                    {
                        Causa02 = db.Causa2.FirstOrDefault(c => c.Id == acr.IdCausa2).Id;
                    }
                    if (db.Causa3.FirstOrDefault(c => c.Id == acr.IdCausa3) == null)
                    {
                        Causa03 = null;
                    }
                    else
                    {
                        Causa03 = db.Causa3.FirstOrDefault(c => c.Id == acr.IdCausa3).Id;
                    }
                    if (db.Causa4.FirstOrDefault(c => c.Id == acr.IdCausa4) == null)
                    {
                        Causa04 = null;
                    }
                    else
                    {
                        Causa04 = db.Causa4.FirstOrDefault(c => c.Id == acr.IdCausa4).Id;
                    }
                    if (db.Causa5.FirstOrDefault(c => c.Id == acr.IdCausa5) == null)
                    {
                        Causa05 = null;
                    }
                    else
                    {
                        Causa05 = db.Causa5.FirstOrDefault(c => c.Id == acr.IdCausa5).Id;
                    }
                    if (db.CausaIntermedia1.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia1) == null)
                    {
                        CausaIntermedia01 = null;
                    }
                    else
                    {
                        CausaIntermedia01 = db.CausaIntermedia1.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia1).Id;
                    }
                    if (db.CausaIntermedia2.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia2) == null)
                    {
                        CausaIntermedia02 = null;
                    }
                    else
                    {
                        CausaIntermedia02 = db.CausaIntermedia2.FirstOrDefault(c => c.Id == acr.IdCausaIntermedia2).Id;
                    }
                }               
            }
            string TipoEvento0;
            string Cliente0;
            string Equipo0;
            string Modelo0;
            string Componente0;
            string Area0;
            string SubArea0;
            string Parte0;
            if (db.TipoEvento.FirstOrDefault(c => c.Id == EventoVariable.TipoEvento) == null)
            {
                TipoEvento0 = null;
            }
            else
            {
                TipoEvento0 = db.TipoEvento.FirstOrDefault(c => c.Id == EventoVariable.TipoEvento).Tipo;
            }

            if (db.Cliente.FirstOrDefault(c => c.Id == EventoVariable.Cliente) == null)
            {
                Cliente0 = null;
            }
            else
            {
                Cliente0 = db.Cliente.FirstOrDefault(c => c.Id == EventoVariable.Cliente).Nombre;
            }

            if (db.Equipo.FirstOrDefault(c => c.Id == EventoVariable.Equipo) == null)
            {
                Equipo0 = null;
            }
            else
            {
                Equipo0 = db.Equipo.FirstOrDefault(c => c.Id == EventoVariable.Equipo).Nombre;
            }

            if (db.Modelo.FirstOrDefault(c => c.Id == EventoVariable.Modelo) == null)
            {
                Modelo0 = null;
            }
            else
            {
                Modelo0 = db.Modelo.FirstOrDefault(c => c.Id == EventoVariable.Modelo).Nombre;
            }

            if (db.Componente.FirstOrDefault(c => c.Id == EventoVariable.Componente) == null)
            {
                Componente0 = null;
            }
            else
            {
                Componente0 = db.Componente.FirstOrDefault(c => c.Id == EventoVariable.Componente).Nombre;
            }

            if (db.Parte.FirstOrDefault(c => c.Id == EventoVariable.Parte) == null)
            {
                Parte0 = null;
            }
            else
            {
                Parte0 = db.Parte.FirstOrDefault(c => c.Id == EventoVariable.Parte).Nombre;
            }

            if (db.Area.FirstOrDefault(c => c.Id == EventoVariable.Area) == null)
            {
                Area0 = null;
            }
            else
            {
                Area0 = db.Area.FirstOrDefault(c => c.Id == EventoVariable.Area).Nombre;
            }

            if (db.SubArea.FirstOrDefault(c => c.Id == EventoVariable.SubArea) == null)
            {
                SubArea0 = null;
            }
            else
            {
                SubArea0 = db.SubArea.FirstOrDefault(c => c.Id == EventoVariable.SubArea).Nombre;
            }

            string nombrecompletojefe = null;
            if (EventoVariable.RutJefeInvolucrado != null)
            {
                nombrecompletojefe = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " +c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(EventoVariable.RutJefeInvolucrado)).nombreCompleto;
            }
            EventoSecuenciaModels EventoSecuenciaModels;
            Persona PersonaUnicaPrueba;
            if (currentUser.IsInRole("Admins"))
            {
                CapturandoUsuario = "Administrador";
                string creadorEvento = null;
                if (db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(EventoVariable.Creador)) != null)
                {
                    creadorEvento = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(EventoVariable.Creador)).nombreCompleto;

                }
                else
                {
                    creadorEvento = "Administrador";
                }
               
                
                List<Persona> listapersonaAI = new List<Persona>();
                foreach (AccionInmediata ai in db.AccionInmediata.ToList())
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(ai.RutPersona.TrimEnd()));
                    listapersonaAI.Add(persona);
                }

                List<Persona> listapersonaAC = new List<Persona>();
                foreach (AccionCorrectiva ac in db.AccionCorrectiva.ToList().Where(c => c.EventoId.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(ac.RutPersona.TrimEnd()));
                    listapersonaAC.Add(persona);
                }

                List<Persona> listapersonaI = new List<Persona>();
                foreach (PersonaEventoInvolucrado pei in db.PersonaEventoInvolucrado.ToList().Where(c => c.IdEvento.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pei.RutPersona.TrimEnd()));
                    listapersonaI.Add(persona);
                }

                List<Persona> listapersonaR = new List<Persona>();
                foreach (PersonaEvento pe in db.PersonaEvento.ToList().Where(c => c.IdEvento.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pe.RutPersona.TrimEnd()));
                    listapersonaR.Add(persona);
                }

                AnalisisCausaRaiz RCA;
                RCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == EventoID && !c.Removed == true);
                string creadorrca = null;
                if (RCA != null)
                {
                    if (RCA.Creador != null)
                    {
                        creadorrca = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(RCA.Creador)).nombreCompleto;
                    }
                    else
                    {
                        creadorrca = "Administrador";
                    }
                }
                RespuestaCliente rc;
                rc = db.RespuestaCliente.FirstOrDefault(c=>c.IdEvento.TrimEnd()==EventoID.TrimEnd()&&c.Removed!=true);

                List<Persona> listaAIInvolucrado = new List<Persona>();
                foreach (AccionInmediataPersona aii in db.AccionInmediataPersona.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(aii.RutPersona.TrimEnd()));
                    listaAIInvolucrado.Add(persona2);
                }
                List<Persona> listaJefeSupervisor = new List<Persona>();
                foreach (PersonaArea pa in db.PersonaArea.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pa.RutPersona.TrimEnd()));
                    listaJefeSupervisor.Add(persona2);
                }
                foreach (PersonaSubArea psa in db.PersonaSubArea.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(psa.RutPersona.TrimEnd()));
                    listaJefeSupervisor.Add(persona2);
                }

                EventoSecuenciaModels = new EventoSecuenciaModels()
                {
                    Creador = creadorEvento,
                    CreadorRCA = creadorrca,
                    TipoEvento = TipoEvento0,
                    Cliente = Cliente0,
                    Equipo = Equipo0,
                    Modelo = Modelo0,
                    Componente = Componente0,
                    parte = Parte0,
                    EventoUnico = db.Evento.FirstOrDefault(c => c.Id == EventoID),
                    
                    ListaAccionesInmediatas = db.AccionInmediata.Where(c => c.EventoId == EventoID && !c.Removed == true).ToList(),
                    ListaInvolucradosAccionInmediata = listaAIInvolucrado,
                    ListaPersonasAccionInmediata = listapersonaAI,
                    ListaPersonasAccionCorrectiva = listapersonaAC,
                    ListaPersonasInvolucrados = listapersonaI,
                    ListaPersonasResponsable = listapersonaR,
                    //ListaPersonas = db.Persona.ToList(),
                    //ListaSupervisores = db.Persona.ToList().Where(c => c.IdAreaJefeArea != null || c.IdSubAreaJefeSubArea != null),
                    ListaSupervisores = listaJefeSupervisor.Distinct(),
                    ListaArchivos = db.Archivo.ToList().Where(c => c.Identificador.TrimEnd().Equals(EventoID)),
                    ListaPersonaEvento = db.PersonaEvento.ToList().Where(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    ListaPersonaEventoInvolucrado = db.PersonaEventoInvolucrado.ToList().Where(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    ListaMensajes = db.Mensajes.ToList().Where(c => !c.Removed == true),
                    analisis = db.Analisis.FirstOrDefault(c => c.EventoId == EventoID),
                    RCA = RCA,
                    RespuestaUnica = rc,
                    ListaCausas = db.Causa.Where(c => c.IdRca == db.AnalisisCausaRaiz.FirstOrDefault(d => d.IdEvento == EventoID).Id).ToList(),
                    ListaEstado = db.Estado.ToList(),
                    ListaOrigenFalla = db.OrigenFalla.ToList(),
                    ListaAccionCorrectiva = db.AccionCorrectiva.ToList().Where(c => c.EventoId.TrimEnd() == EventoID).OrderBy(d => d.FechaRegistro),
                    Evaluacion = db.Evaluacion.FirstOrDefault(c=>c.IdEvento.TrimEnd().Equals(EventoID)),
                    Verificacion = db.Verificacion.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    Area = Area0,
                    SubArea = SubArea0,
                    Causa1 = Causa01,
                    Causa2 = Causa02,
                    Causa3 = Causa03,
                    Causa4 = Causa04,
                    Causa5 = Causa05,
                    CausaIntermedia1 = CausaIntermedia01,
                    CausaIntermedia2 = CausaIntermedia02,
                    ListaCausa1 = db.Causa1.ToList(),
                    ListaCausa2 = db.Causa2.ToList(),
                    ListaCausa3 = db.Causa3.ToList(),
                    ListaCausa4 = db.Causa4.ToList(),
                    ListaCausa5 = db.Causa5.ToList(),
                    ListaCausaIntermedia1 = db.CausaIntermedia1.ToList(),
                    ListaCausaIntermedia2 = db.CausaIntermedia2.ToList(),
                    OrigenFalla = OrigenFalla0,
                    ResponsableInvolucradoNombre = nombrecompletojefe,
                    ListaProceso1 = db.Proceso1.ToList(),
                    ListaProceso2 = db.Proceso2.ToList(),
                    ListaProceso3 = db.Proceso3.ToList(),
                    ListaProceso4 = db.Proceso4.ToList(),
                    ListaProceso5 = db.Proceso5.ToList(),
                    ListaFallaPrimaria = db.FallaPrimaria.ToList(),
                    ListaFallaSecundaria = db.FallaSecundaria.ToList(),
                    FallaPrimaria = FallaPrimaria01,
                    FallaSecundaria = FallaSecundaria01,
                };
            }
            else
            {
                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,
                    }
                };
                PersonaUnicaPrueba = db.Persona.FirstOrDefault(c => c.Rut == db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona);
                CapturandoUsuario = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona)).nombreCompleto;
                string creadorEvento = null;
                if (db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(EventoVariable.Creador))!=null)
                {
                 creadorEvento = db.Persona.Select(c => new { nombreCompleto = c.Nombre + c.ApellidoPaterno + c.ApellidoMaterno, rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(EventoVariable.Creador)).nombreCompleto;

                }
                else
                {
                    creadorEvento = "Administrador";
                }
                List<Persona> listapersonaAI = new List<Persona>();
         
                foreach (AccionInmediata ai in db.AccionInmediata.ToList().Where(c=>c.EventoId.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(ai.RutPersona.TrimEnd()));
                    listapersonaAI.Add(persona);
                }

                List<Persona> listapersonaAC = new List<Persona>();
                foreach (AccionCorrectiva ac in db.AccionCorrectiva.ToList().Where(c => c.EventoId.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(ac.RutPersona.TrimEnd()));
                    listapersonaAC.Add(persona);
                }

                List<Persona> listapersonaI = new List<Persona>();
                foreach (PersonaEventoInvolucrado pei in db.PersonaEventoInvolucrado.ToList().Where(c => c.IdEvento.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pei.RutPersona.TrimEnd()));
                    listapersonaI.Add(persona);
                }

                List<Persona> listapersonaR = new List<Persona>();
                foreach (PersonaEvento pe in db.PersonaEvento.ToList().Where(c => c.IdEvento.Equals(EventoVariable.Id)))
                {
                    Persona persona = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pe.RutPersona.TrimEnd()));
                    listapersonaR.Add(persona);
                }
                AnalisisCausaRaiz RCA;
                RCA = db.AnalisisCausaRaiz.FirstOrDefault(c => c.IdEvento == EventoID && !c.Removed == true);
                string creadorrca = null;
                if (RCA != null)
                {
                    if (RCA.Creador != null)
                    {
                        creadorrca = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.Equals(RCA.Creador)).nombreCompleto;
                    }
                    else
                    {
                        creadorrca = "Administrador";
                    }
                }
                RespuestaCliente rc;
                rc = db.RespuestaCliente.FirstOrDefault(c => c.IdEvento.TrimEnd() == EventoID.TrimEnd() && c.Removed != true);

                List<Persona> listaAIInvolucrado = new List<Persona>();
                foreach (AccionInmediataPersona aii in db.AccionInmediataPersona.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(aii.RutPersona.TrimEnd()));
                    listaAIInvolucrado.Add(persona2);
                }
                List<Persona> listaJefeSupervisor = new List<Persona>();
                foreach (PersonaArea pa in db.PersonaArea.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(pa.RutPersona.TrimEnd()));
                    listaJefeSupervisor.Add(persona2);
                }
                foreach (PersonaSubArea psa in db.PersonaSubArea.ToList())
                {
                    Persona persona2 = db.Persona.FirstOrDefault(c => c.Rut.TrimEnd().Equals(psa.RutPersona.TrimEnd()));
                    listaJefeSupervisor.Add(persona2);
                }
                EventoSecuenciaModels = new EventoSecuenciaModels()
                {
                    Creador = creadorEvento,
                    CreadorRCA=creadorrca,
                    TipoEvento = TipoEvento0,
                    Cliente = Cliente0,
                    Equipo = Equipo0,
                    Modelo = Modelo0,
                    Componente = Componente0,
                    parte = Parte0,
                    EventoUnico = db.Evento.FirstOrDefault(c => c.Id == EventoID),
                    ListaAccionesInmediatas = db.AccionInmediata.Where(c => c.EventoId == EventoID && !c.Removed == true).ToList(),
                    ListaInvolucradosAccionInmediata = listaAIInvolucrado,
                    ListaPersonasAccionInmediata = listapersonaAI,
                    ListaPersonasAccionCorrectiva= listapersonaAC,
                    ListaPersonasInvolucrados = listapersonaI,
                    ListaPersonasResponsable = listapersonaR,
                    //ListaPersonas = db.Persona.ToList(),
                    //ListaSupervisores = db.Persona.ToList().Where(c => c.IdAreaJefeArea != null || c.IdSubAreaJefeSubArea != null),
                    ListaSupervisores = listaJefeSupervisor.Distinct(),
                    ListaArchivos = db.Archivo.ToList().Where(c => c.Identificador.TrimEnd().Equals(EventoID)),
                    ListaPersonaEvento = db.PersonaEvento.ToList().Where(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    ListaPersonaEventoInvolucrado = db.PersonaEventoInvolucrado.ToList().Where(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    ListaMensajes = db.Mensajes.ToList().Where(c => !c.Removed == true),
                    analisis = db.Analisis.FirstOrDefault(c => c.EventoId == EventoID),
                    PersonaUnica = PersonaUnicaPrueba,
                    ResponsableInvolucradoNombre = nombrecompletojefe,
                    RCA = RCA,
                    RespuestaUnica = rc,
                    ListaCausas = db.Causa.Where(c => c.IdRca == db.AnalisisCausaRaiz.FirstOrDefault(d => d.IdEvento == EventoID).Id).ToList(),
                    ListaEstado = db.Estado.ToList(),
                    ListaOrigenFalla = db.OrigenFalla.ToList(),
                    ListaAccionCorrectiva = db.AccionCorrectiva.ToList().Where(c => c.EventoId.TrimEnd() == EventoID).OrderBy(d => d.FechaRegistro),
                    Evaluacion = db.Evaluacion.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    Verificacion = db.Verificacion.FirstOrDefault(c => c.IdEvento.TrimEnd().Equals(EventoID)),
                    ListaArea = db.Area.ToList().Where(c=>c.Removed!=true),
                    ListaSubArea = db.SubArea.ToList().Where(c => c.Removed != true),
                    Area = Area0,
                    SubArea = SubArea0,
                    Causa1 = Causa01,
                    Causa2 = Causa02,
                    Causa3 = Causa03,
                    Causa4 = Causa04,
                    Causa5 = Causa05,
                    CausaIntermedia1 = CausaIntermedia01,
                    CausaIntermedia2 = CausaIntermedia02,
                    ListaCausa1 = db.Causa1.ToList(),
                    ListaCausa2 = db.Causa2.ToList(),
                    ListaCausa3 = db.Causa3.ToList(),
                    ListaCausa4 = db.Causa4.ToList(),
                    ListaCausa5 = db.Causa5.ToList(),
                    ListaCausaIntermedia1 = db.CausaIntermedia1.ToList(),
                    ListaCausaIntermedia2 = db.CausaIntermedia2.ToList(),
                    OrigenFalla = OrigenFalla0,

                    ListaProceso1 = db.Proceso1.ToList(),
                    ListaProceso2 = db.Proceso2.ToList(),
                    ListaProceso3 = db.Proceso3.ToList(),
                    ListaProceso4 = db.Proceso4.ToList(),
                    ListaProceso5 = db.Proceso5.ToList(),

                    ListaFallaPrimaria = db.FallaPrimaria.ToList(),
                    ListaFallaSecundaria = db.FallaSecundaria.ToList(),
                    FallaPrimaria = FallaPrimaria01,
                    FallaSecundaria = FallaSecundaria01,

                };
            }




            return View(EventoSecuenciaModels);
        }
        public JsonResult getListaSupervisor(int idArea)
        {
             
            List<Persona> list = new List<Persona>();
            List<SubArea> listSubArea = new List<SubArea>();
            int contador = 0;
            foreach (SubArea subarea in listSubArea.Where(c=>c.IdArea==idArea))
            {
                foreach (Persona persona in list.Where(c => c.IdArea==subarea.Id))
                {
                    list.Insert(contador, new Persona {Rut=persona.Rut,Nombre=persona.Nombre } );
                    contador++;
                }

            }
            //list = db.Persona.Where(c => c.IdSubAreaJefeSubArea==idSubArea).ToList();
            //list.Insert(0, new Modelo { Id = 0, Nombre = "Please Select Modelo" });
            return Json(new SelectList(list));
        }
        public JsonResult getPersonaResponsableModalAjax(string valor)
        {
            IEnumerable<Area> ListaArea = db.Area.ToList();
            IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();

            List<Persona> list = new List<Persona>();
            list = db.Persona.Where(c => (c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd()).ToUpper().Contains(valor.ToUpper())).ToList();
            List<DatosPersona> Datoslist = new List<DatosPersona>();
            int contador = 0;
            foreach (Persona persona in list)
            {
                string areaVariable = "";
                string SubAreaVariable = "";
                string nombreCompleto = "";
                string cargoVariable = "";

                if (ListaArea.FirstOrDefault(c => c.Id == persona.IdArea) != null)
                {
                    areaVariable = ListaArea.FirstOrDefault(c => c.Id == persona.IdArea).Nombre;
                }
                if (ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea) != null)
                {
                    SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea).Nombre;
                }
                if (persona.Cargo != null)
                {
                    cargoVariable = persona.Cargo;
                }
                nombreCompleto = persona.Nombre + persona.ApellidoPaterno + persona.ApellidoMaterno;
                Datoslist.Insert(contador, new DatosPersona { rut = persona.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                contador++;
            }

            //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
            //return Json(new SelectList(list,"Rut", "ApellidoPaterno"));
            return Json(Datoslist);
        }

        public JsonResult getPersonaInvolucradoAI(string id)
        {
            IEnumerable<Area> ListaArea = db.Area.ToList();
            IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();
            IEnumerable<AccionInmediataPersona> ListaInvolucrados = db.AccionInmediataPersona.ToList().Where(c =>c.IdAccionInmediata.TrimEnd().Equals(id.TrimEnd()) && c.Removed!=true);

            AccionInmediata ai = db.AccionInmediata.FirstOrDefault(c=>c.Id.TrimEnd().Equals(id.TrimEnd()));

            //List<Persona> list = new List<Persona>();
            //list = db.Persona.Where(c => (c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd()).ToUpper().Contains(valor.ToUpper())).ToList();
            List<DatosPersona> Datoslist = new List<DatosPersona>();
            int contador = 0;
            foreach (AccionInmediataPersona lai in ListaInvolucrados)
            {
                Persona per = db.Persona.FirstOrDefault(c=>c.Rut.TrimEnd().Equals(lai.RutPersona.TrimEnd()));
                string areaVariable = "";
                string SubAreaVariable = "";
                string nombreCompleto = "";
                string cargoVariable = "";

                if (ListaArea.FirstOrDefault(c => c.Id == per.IdArea) != null)
                {
                    areaVariable = ListaArea.FirstOrDefault(c => c.Id == per.IdArea).Nombre;
                }
                if (ListaSubArea.FirstOrDefault(c => c.Id == per.IdSubArea) != null)
                {
                    SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == per.IdSubArea).Nombre;
                }
                if (per.Cargo != null)
                {
                    cargoVariable = per.Cargo;
                }
                nombreCompleto = per.Nombre + per.ApellidoPaterno + per.ApellidoMaterno;
                Datoslist.Insert(contador, new DatosPersona { rut = per.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                contador++;
            }

            //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
            //return Json(new SelectList(list,"Rut", "ApellidoPaterno"));
            return Json(Datoslist);
        }


        public ActionResult ModalAction(int Id)
        {
            ViewBag.Id = Id;
            return PartialView("ModalContent");
        }

        public JsonResult getPersonaModalAjax(string valor)
        {
            IEnumerable<Area> ListaArea = db.Area.ToList();
            IEnumerable<SubArea> ListaSubArea = db.SubArea.ToList();

            List<Persona> list = new List<Persona>();
            list = db.Persona.Where(c => (c.Nombre.TrimEnd()+" "+c.ApellidoPaterno.TrimEnd()+" "+c.ApellidoMaterno.TrimEnd()).ToUpper().Contains(valor.ToUpper())).ToList();
            List<DatosPersona> Datoslist = new List<DatosPersona>();
            int contador = 0;
            foreach (Persona persona in list){
                string areaVariable = "";
                string SubAreaVariable = "";
                string nombreCompleto = "";
                string cargoVariable = "";

                if (ListaArea.FirstOrDefault(c => c.Id == persona.IdArea)!=null)
                {
                    areaVariable = ListaArea.FirstOrDefault(c => c.Id == persona.IdArea).Nombre;
                }
                if (ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea) != null)
                {
                    SubAreaVariable = ListaSubArea.FirstOrDefault(c => c.Id == persona.IdSubArea).Nombre;
                }
                if (persona.Cargo != null)
                {
                    cargoVariable = persona.Cargo;
                }
                nombreCompleto = persona.Nombre + persona.ApellidoPaterno + persona.ApellidoMaterno;
                Datoslist.Insert(contador, new DatosPersona {rut=persona.Rut,Nombre= nombreCompleto, Area =areaVariable,SubArea = SubAreaVariable, Cargo= cargoVariable });
                contador++;
            }

            //list.Insert(0, new SubArea { Id = 0, Nombre = "Please Select SubArea" });
            //return Json(new SelectList(list,"Rut", "ApellidoPaterno"));
            return Json(Datoslist);
        }

        //public IActionResult getPersonaModalAjax(string valor)
        //{

        //    IEnumerable<Persona> listaPersonas = db.Persona.Where(c => c.Nombre.Equals(valor)).ToList();
        //    return Json(listaPersonas);
        //}
        public async Task<IActionResult> Subida(EventoSecuenciaModels esm, IFormFile Nombre)
        {
            CreateParte parteFile = new CreateParte();

            //string pruebaId = "IdPrueba";
            string direccionEventoId = esm.EventoUnico.Id.TrimEnd();

            if (Nombre != null)
            {
                string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
                Directory.CreateDirectory(Path.Combine(uploadPath, direccionEventoId));

                string filename = Nombre.FileName;
                using (FileStream fs = new FileStream(Path.Combine(uploadPath, direccionEventoId, filename), FileMode.Create))
                {
                    await Nombre.CopyToAsync(fs);
                }

                Archivo ar = new Archivo()
                {
                    Identificador = esm.EventoUnico.Id.TrimEnd(),
                    Nombre = filename,
                    Tipo = "Evento",
                    Removed = false,
                    FechaRegistro = DateTime.Now,
                };
                db.Archivo.Add(ar);
                db.SaveChanges();

            }
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = esm.EventoUnico.Id.TrimEnd() });
            //return View(parteFile);
        }



        public async Task<IActionResult> Download(string filename, string Identificador, string tipo)
        {
            if (filename == null)
                return Content("filename not present");

            //string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
            //Directory.CreateDirectory(Path.Combine(uploadPath, Id));



            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads", Identificador.TrimEnd(), filename.TrimEnd());

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        public async Task<IActionResult> DownloadArchivoRespuestaCliente(string filename, string Identificador, string tipo)
        {
            if (filename == null)
                return Content("filename not present");

            //string uploadPath = Path.Combine(_enviroment.WebRootPath, "uploads");
            //Directory.CreateDirectory(Path.Combine(uploadPath, Id));



            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "uploads", Identificador.TrimEnd()+"/RespuestaCliente", filename.TrimEnd());

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                //------Documentos------
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".txt", "text/plain"},
                {".csv", "text/csv"},
                {".pdf", "application/pdf"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".odp", "application/vnd.oasis.opendocument.presentation"},
                {".ods", "	application/vnd.oasis.opendocument.spreadsheet"},
                {".odt", "	application/vnd.oasis.opendocument.text"},
                {".vsd", "application/vnd.visio"},
                //---PPT----
                {".pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                {".ppt","application/vnd.ms-powerpoint" },
                {".potx","application/vnd.openxmlformats-officedocument.presentationml.template" },

                //-------Imagen-------
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".ico", "image/x-icon"},
                {".webp", "image/webp"},
              
                //---Video-----
                {".avi","video/*" },
                {".mp4","video/*" },
                {".mpeg","video/*" },
                {".mpg","video/*" },
                {".ogv","video/ogg" },
                {".webm","video/webm" },
                {".gif","image/gif" },
               
               
                //---Audio
                {".mp3","audio/*" },
                {".wav","audio/x-wav" },
                {".oga","audio/ogg" },
                {".aac","audio/aac" },
                {".mid","audio/midi" },
                {".midi","audio/midi" },
                {".weba","audio/webm" },
      
                //----zip y rar----
                {".rar","application/x-rar-compressed" },
                {".zip","application/zip" },
                {".bz","application/x-bzip" },
                {".bz2","application/x-bzip2" },

            };
        }

        public IActionResult DeleteArchivo(string EventoId, int FileId)
        {
            Archivo DeleteArchivo = db.Archivo.FirstOrDefault(c => c.FileId == FileId);
            DeleteArchivo.Removed = true;
            db.Archivo.Update(DeleteArchivo);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = EventoId });

        }


        public IActionResult RegistrarMensaje(EventoSecuenciaModels es)
        {
            ClaimsPrincipal currentUser = User;
            string CapturandoUsuario = null;
            PruebaUser prueba;

            if (currentUser.IsInRole("Admins"))
            {
                CapturandoUsuario = "Administrador";
            }
            else
            {
                prueba = new PruebaUser()
                {
                    ListaUsuarios = db.AspNetUsers.ToList(),
                    Usuario = new AspNetUsers()
                    {
                        Id = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value,
                    }
                };

                CapturandoUsuario = db.Persona.FirstOrDefault(c => c.Rut.Equals(db.AspNetUsers.FirstOrDefault(d => d.Id.Equals(prueba.Usuario.Id)).RutPersona)).Rut;
            }


            Mensajes mensaje = new Mensajes()
            {

                IdEvento = es.EventoUnico.Id,
                RutPersona = CapturandoUsuario,
                FechaRegistro = DateTime.Now,
                Descripcion = es.descripcion,
                Removed = false,
                Tipo = "Evento",
            };
            db.Mensajes.Add(mensaje);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = es.EventoUnico.Id });

        }

        public IActionResult DeleteMensaje(string EventoID, int MensajeID)
        {

            Mensajes updateMensajes = db.Mensajes.FirstOrDefault(c => c.Id == MensajeID);
            updateMensajes.Removed = true;

            db.Mensajes.Update(updateMensajes);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = EventoID.TrimEnd() });

        }

        public IActionResult DeleteResponsable(string eventoID, string ResponsableRut)
        {

            PersonaEvento DeleteResponsable = db.PersonaEvento.FirstOrDefault(c => c.IdEvento == eventoID.TrimEnd() && c.RutPersona == ResponsableRut.TrimEnd() && c.Estado == 1);
            //DeleteResponsable.Removed = true;
            //db.PersonaEvento.Update(DeleteResponsable);
            //db.SaveChanges();
            db.PersonaEvento.Remove(DeleteResponsable);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoID.TrimEnd() });


        }
        public IActionResult DeleteInvolucradoAI(string eventoID, string InvolucradoRut)
        {

            AccionInmediataPersona DeleteInvolucrado = db.AccionInmediataPersona.FirstOrDefault(c => c.IdEvento == eventoID.TrimEnd() && c.RutPersona == InvolucradoRut.TrimEnd() );
            //DeleteInvolucrado.Removed = true;
            //db.AccionInmediataPersona.Update(DeleteInvolucrado);
            //db.SaveChanges();



            db.AccionInmediataPersona.Remove(DeleteInvolucrado);
            db.SaveChanges();

            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoID.TrimEnd() });


        }
        public IActionResult AsignarInvolucradoModal(string Rut, string IdEvento)
        {
            PersonaEventoInvolucrado pe = new PersonaEventoInvolucrado()
            {

                RutPersona = Rut.TrimEnd(),
                IdEvento = IdEvento.TrimEnd(),
                Removed = false,
                FechaRegistro = DateTime.Now,
                Estado = 4,


            };
            db.PersonaEventoInvolucrado.Add(pe);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = IdEvento.TrimEnd() });

        }
        public JsonResult ComprobarInvolucrado(string Rut, string IdEvento)
        {
            int comprobar = 0;
            if (db.PersonaEventoInvolucrado.FirstOrDefault(c=>c.RutPersona.TrimEnd().Equals(Rut.TrimEnd())&& c.IdEvento.TrimEnd().Equals(IdEvento.TrimEnd()))!=null)
            {
                comprobar = 1;
            }
            else
            {
                comprobar = 0;
            }
            return Json(comprobar);


        }
        public JsonResult ConfirmarUsuario(string IdEvento)
        {
            ClaimsPrincipal currentUser = User;
            if (currentUser.IsInRole("Admins") || currentUser.IsInRole("Calidad"))
            {
                string comprobar = "Autorizado";
                return Json(comprobar);
            }
            else
            {
                string comprobar = "Denegado";
                return Json(comprobar);
            }

   
    


        }
        public JsonResult ComprobarResponsableRCA(string Rut, string IdEvento)
        {
            int comprobar = 0;
            if (db.PersonaEvento.FirstOrDefault(c => c.RutPersona.TrimEnd().Equals(Rut.TrimEnd())&&c.Estado==4 && c.IdEvento.TrimEnd().Equals(IdEvento.TrimEnd())) != null)
            {
                comprobar = 1;
            }
            else
            {
                comprobar = 0;
            }
            return Json(comprobar);


        }
        public JsonResult ComprobarInvolucradoAI(string Rut, string IdEvento, string IDAI)
        {
            int comprobar = 0;
            if (db.AccionInmediataPersona.FirstOrDefault(c => c.RutPersona.TrimEnd().Equals(Rut.TrimEnd()) && c.IdEvento.TrimEnd().Equals(IdEvento.TrimEnd()) && c.IdAccionInmediata.TrimEnd().Equals(IDAI.TrimEnd())) != null)
            {
                comprobar = 1;
            }
            else
            {
                comprobar = 0;
            }
            return Json(comprobar);


        }
        public IActionResult AsignarInvolucradoAI(string Rut, string IdEvento, string Id)
        {
            ViewBag.Prueba = 2;


            AccionInmediataPersona pe = new AccionInmediataPersona()
            {

                RutPersona = Rut,
                IdAccionInmediata = Id,
                IdEvento = IdEvento,
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",



            };
            db.AccionInmediataPersona.Add(pe);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = IdEvento });
        }

        public async Task<List<AccionInmediata>> AIModalAjax(string id)
        {
            List<AccionInmediata> le = new List<AccionInmediata>();
            var ai = db.AccionInmediata.FirstOrDefault(c => c.Id == id);
            le.Add(ai);
            return le;

        }

        public IActionResult AsignarResponsableModalRCA(string Rut, string IdEvento)
        {
            ViewBag.Prueba = 2;


            PersonaEvento pe = new PersonaEvento()
            {

                RutPersona = Rut,
                IdEvento = IdEvento,
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 4,


            };
            db.PersonaEvento.Add(pe);
            //db.SaveChanges();

            PersonaEvento pe2 = new PersonaEvento()
            {
                RutPersona = Rut,
                IdEvento = IdEvento,
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 5,


            };
            db.PersonaEvento.Add(pe2);
            //db.SaveChanges();

            PersonaEvento pe3 = new PersonaEvento()
            {

                RutPersona = Rut,
                IdEvento = IdEvento,
                Removed = false,
                FechaRegistro = DateTime.Now,
                //Etapa = "RCA",
                Estado = 6,


            };
            db.PersonaEvento.Add(pe3);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = IdEvento });
        }





        public IActionResult AgregarResponsableInvolucrado(EventoSecuenciaModels model)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == model.EventoUnico.Id);
            UpdateEvento.RutJefeInvolucrado = model.ResponsableInvolucrado;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = model.EventoUnico.Id.TrimEnd() });

        }
        public IActionResult AprobarAccionInmediata(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 3;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();
         
            return Json("Aprobado");
            //return RedirectToAction("Index", "EventoSecuencia", new { EventoID = eventoid });

        }

        public IActionResult AprobarRegistroInvolucrados(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 6;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return Json("Aprobado");

        }

        public IActionResult AprobarAccionCorrectiva(string eventoid)
        {
            Evento UpdateEvento = db.Evento.FirstOrDefault(c => c.Id == eventoid);
            UpdateEvento.Estado = 7;
            db.Evento.Update(UpdateEvento);
            db.SaveChanges();


            return Json("Aprobado");

        }

    }
}
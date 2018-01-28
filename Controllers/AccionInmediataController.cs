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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize]
    public class AccionInmediataController : Controller
    {
        private UserManager<AppUser> userManager;
        public AccionInmediataController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        private AdventureWorksContext db = new AdventureWorksContext();
        // GET: /<controller>/


        public async Task<IActionResult> FormularioCrearAccionInmediata(string eventoID)
        {
            AccionInmediataModels FormularioCrearAcccionInmediata = new AccionInmediataModels()
            {
                EventoId = eventoID,
                //ListaPersonas = db.Persona.ToList(),
            };
            return View(FormularioCrearAcccionInmediata);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioCrearAccionInmediata(AccionInmediataModels AI)
        {
            if (ModelState.IsValid)
            { 
            AccionInmediata Accion = new AccionInmediata()
            {
                FechaRegistro = DateTime.Now,
                Descripcion = AI.Descripcion,
                Removed = false,
                EventoId = AI.EventoId,
                RutPersona = AI.RutPersona,
                Efectivo = AI.Efectiva,
            };
            db.AccionInmediata.Add(Accion);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = AI.EventoId });
            }
            else
            {
                return await FormularioCrearAccionInmediata(AI.EventoId.TrimEnd());
            }
        }

        public async Task<IActionResult> FormularioEditAccionInmediata(string AccionInmediataID)
        {
         AccionInmediata AI = db.AccionInmediata.FirstOrDefault(c => c.Id == AccionInmediataID);

          AccionInmediataModels Accion = new AccionInmediataModels()
            {
                AccionUnica = AI,
                //ListaPersonas = db.Persona.ToList().Where(c => !c.Removed == true),
                EventoId = (db.AccionInmediata.FirstOrDefault(c => c.Id == AccionInmediataID).EventoId),
                RutPersona = AI.RutPersona,
          };


            string nombrecompleto = null;
            if (Accion.AccionUnica.RutPersona != null)
            {
                nombrecompleto = db.Persona.Select(c => new { nombreCompleto = c.Nombre.TrimEnd() + " " + c.ApellidoPaterno.TrimEnd() + " " + c.ApellidoMaterno.TrimEnd(), rut = c.Rut }).FirstOrDefault(c => c.rut.TrimEnd().Equals(Accion.AccionUnica.RutPersona.TrimEnd())).nombreCompleto;
            }
            Accion.NombreCompleto = nombrecompleto;

            return View(Accion);
        }
        [HttpPost]
        public async Task<IActionResult> FormularioEditAccionInmediata(AccionInmediataModels accion)
        {
            if (ModelState.IsValid)
            {
                AccionInmediata updateAccionInmediata = db.AccionInmediata.FirstOrDefault(c => c.Id == accion.AccionUnica.Id);
                updateAccionInmediata.Descripcion = accion.Descripcion;
                updateAccionInmediata.RutPersona = accion.RutPersona;
                updateAccionInmediata.Efectivo = accion.Efectiva;
                db.AccionInmediata.Update(updateAccionInmediata);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = accion.AccionUnica.EventoId.TrimEnd() });
            }
            else
            {
                return await FormularioEditAccionInmediata(accion.AccionUnica.Id);
            }
        }


        public IActionResult DeleteAccionInmediata(string AccioninmediataID)
        {
            AccionInmediata updateAccionInmediata = db.AccionInmediata.FirstOrDefault(c => c.Id == AccioninmediataID);
            updateAccionInmediata.Removed = true;
            db.AccionInmediata.Update(updateAccionInmediata);
            db.SaveChanges();
            return RedirectToAction("Index", "EventoSecuencia", new { EventoID = updateAccionInmediata.EventoId });

        }
        public JsonResult getPersonaModalAjax(string valor)
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
                nombreCompleto = persona.Nombre.TrimEnd()+" " + persona.ApellidoPaterno.TrimEnd()+" " + persona.ApellidoMaterno.TrimEnd();
                Datoslist.Insert(contador, new DatosPersona { rut = persona.Rut, Nombre = nombreCompleto, Area = areaVariable, SubArea = SubAreaVariable, Cargo = cargoVariable });
                contador++;
            }

            return Json(Datoslist);
        }
    }
}
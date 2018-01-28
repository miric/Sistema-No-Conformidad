using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FINNINGWEB.Models;
using Microsoft.AspNetCore.Identity;
using FINNINGWEB.Entities;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FINNINGWEB.Controllers
{
    [Authorize]
    public class AnalisisController : Controller
    {

        private UserManager<AppUser> userManager;
        public AnalisisController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        private AdventureWorksContext db = new AdventureWorksContext();



        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CrearFormularioAnalisis(string eventoID)
        {
            AnalisisModels FormularioCrearAnalisis = new AnalisisModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),

            };

            return View(FormularioCrearAnalisis);
        }

        [HttpPost]
        public async Task<IActionResult> CrearFormularioAnalisis(AnalisisModels a)
        {

            if (ModelState.IsValid)
            { 

                Analisis analisis = new Analisis()
            {

                FechaRegistro = a.FechaRegistro,
                AplicaRca = a.AplicaRCA,
                EventoId = a.EventoId,
                Descripcion = a.Descripcion,

                Removed = false,

            };
            db.Analisis.Add(analisis);
            db.SaveChanges();


            if (a.AplicaRCA == false)
            {
                Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == analisis.EventoId);
                updateEvento.Estado = 10;
                updateEvento.Ncaceptada = false;
                db.Evento.Update(updateEvento);
                db.SaveChanges();

            }
            else
            {
                if (a.AplicaRCA == true)
                {
                    Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == analisis.EventoId);
                    updateEvento.Estado = 4;
                    updateEvento.Ncaceptada = null;
                    db.Evento.Update(updateEvento);
                    db.SaveChanges();
                }
            }

                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = a.EventoId });
            }
            else
            {
                return await CrearFormularioAnalisis(a.EventoId.TrimEnd());
            }

    }

        public async Task<IActionResult> EditFormularioAnalisis(string eventoID, string AnalisisID)
        {
            AnalisisModels FormularioCrearAnalisis = new AnalisisModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),
                AnalisisUnico = db.Analisis.FirstOrDefault(c => c.Id == AnalisisID),
                Descripcion = db.Analisis.FirstOrDefault(c => c.Id == AnalisisID).Descripcion,

            };

            return View(FormularioCrearAnalisis);
        }
        [HttpPost]
        public async Task<IActionResult> EditFormularioAnalisis(AnalisisModels analisis)
        {

            if (ModelState.IsValid)
            {
            
            Analisis updateAnalisis = db.Analisis.FirstOrDefault(c => c.Id == analisis.AnalisisUnico.Id);
            updateAnalisis.Descripcion = analisis.Descripcion;
            updateAnalisis.AplicaRca = analisis.AplicaRCA;
            updateAnalisis.FechaRegistro = analisis.FechaRegistro;
            db.Analisis.Update(updateAnalisis);
            db.SaveChanges();

            if (analisis.AplicaRCA == false)
            {
                Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == analisis.EventoId);
                updateEvento.Estado = 10;
                updateEvento.Ncaceptada = false;
                db.Evento.Update(updateEvento);
                db.SaveChanges();
            }
            else
            {
                if (analisis.AplicaRCA == true)
                {
                    Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == analisis.EventoId);
                    updateEvento.Estado = 4;
                    updateEvento.Ncaceptada = null;
                    db.Evento.Update(updateEvento);
                    db.SaveChanges();
                }
            }

                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = analisis.EventoId });
            }
            else
            {
                //string prueba1 = analisis.EventoId.TrimEnd();1
                //string prueba2 = analisis.IdAnalisis;
                return await EditFormularioAnalisis(analisis.EventoId.TrimEnd(),analisis.AnalisisUnico.Id.TrimEnd());
            }
        }

    }
}

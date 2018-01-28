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
    public class EvaluacionController : Controller
    {

        private UserManager<AppUser> userManager;
        public EvaluacionController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        private AdventureWorksContext db = new AdventureWorksContext();


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FormularioCrearEvaluacion(string eventoID)
        {
            EvaluacionModels em = new EvaluacionModels()
            {
                EventoId = eventoID,
                efectivo = null,

            };
            return View(em);
        }


        [HttpPost]
        public async Task<IActionResult> FormularioCrearEvaluacion(EvaluacionModels em)
        {

            if (ModelState.IsValid)
            {
                Evaluacion e = new Evaluacion()
                {
                    IdEvento = em.EventoId,
                    FechaRegistro = em.FechaRegistro,
                    Efectiva = em.efectivo,
                    Descripcion = em.Descripcion,
                    Removed = false,
                };
                db.Evaluacion.Add(e);
                db.SaveChanges();

                if (em.efectivo != null)
                {
                    if (em.efectivo == true)
                    {

                        Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == em.EventoId);
                        updateEvento.Estado = 9;

                        db.Evento.Update(updateEvento);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (em.efectivo == false)
                        {

                            Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == em.EventoId);
                            updateEvento.Estado = 4;

                            db.Evento.Update(updateEvento);
                            db.SaveChanges();

                        }
                    }

                }






                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = em.EventoId });
            }
            else
            {
                return await FormularioCrearEvaluacion(em.EventoId.TrimEnd());
            }
        }


        public async Task<IActionResult> FormularioEditEvaluacion(string eventoID, string EvaluacionID)
        {
            EvaluacionModels em = new EvaluacionModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),
                 EvaluacionUnica = db.Evaluacion.FirstOrDefault(c => c.Id == EvaluacionID),

            };

            return View(em);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditEvaluacion(EvaluacionModels em)
        {

            if (ModelState.IsValid)
            {

                Evaluacion updateEvaluacion = db.Evaluacion.FirstOrDefault(c => c.Id.TrimEnd() == em.EvaluacionUnica.Id);
                updateEvaluacion.FechaRegistro = em.FechaRegistro;
                updateEvaluacion.Efectiva = em.EvaluacionUnica.Efectiva;
                updateEvaluacion.Descripcion = em.Descripcion;
                db.Evaluacion.Update(updateEvaluacion);
                db.SaveChanges();



                if (em.EvaluacionUnica.Efectiva != null)
                {
                    if (em.EvaluacionUnica.Efectiva == true)
                    {

                        Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == em.EventoId);
                        updateEvento.Estado = 9;

                        db.Evento.Update(updateEvento);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (em.EvaluacionUnica.Efectiva == false)
                        {

                            Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == em.EventoId);
                            updateEvento.Estado = 4;

                            db.Evento.Update(updateEvento);
                            db.SaveChanges();

                        }
                    }

                }




                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = em.EventoId.TrimEnd() });
            }
            else
            {
                return await FormularioEditEvaluacion(em.EventoId.TrimEnd(), em.EvaluacionUnica.Id.TrimEnd());
            }
        }


    }
}

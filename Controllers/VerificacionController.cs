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
    public class VerificacionController : Controller
    {
        private UserManager<AppUser> userManager;
        public VerificacionController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        private AdventureWorksContext db = new AdventureWorksContext();


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> FormularioCrearVerificacion(string eventoID)
        {
            VerificacionModels vm = new VerificacionModels()
            {
                EventoId = eventoID,
                efectivo = null,

            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> FormularioCrearVerificacion(VerificacionModels vm)
        {
            if (ModelState.IsValid)
            {
                Verificacion v = new Verificacion()
                {
                    IdEvento = vm.EventoId,
                    FechaRegistro = vm.FechaRegistro,
                    Efectiva = vm.efectivo,
                    Descripcion = vm.Descripcion,
                    Removed = false,
                };
                db.Verificacion.Add(v);
                db.SaveChanges();



                if (vm.efectivo != null)
                {
                    if (vm.efectivo == true)
                    {

                        Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == vm.EventoId);
                        updateEvento.Estado = 10;

                        db.Evento.Update(updateEvento);
                        db.SaveChanges();
                    }
                    else
                    {
                        //verificar si se debe cerrar el evento o abrir uno nuevo con los mismos datos?
                        if (vm.efectivo == false)
                        {

                            Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == vm.EventoId);
                            updateEvento.Estado = 10;

                            db.Evento.Update(updateEvento);
                            db.SaveChanges();

                        }
                    }

                }



                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = vm.EventoId });
            }
            else
            {


                return await FormularioCrearVerificacion(vm.EventoId.TrimEnd());
            }

        }
        public async Task<IActionResult> FormularioEditVerificacion(string eventoID, string VerificacionID)
        {
            VerificacionModels vm = new VerificacionModels()
            {
                EventoId = eventoID,
                ListaPersonas = db.Persona.ToList(),
                VerificacionUnica = db.Verificacion.FirstOrDefault(c => c.Id == VerificacionID),

            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> FormularioEditVerificacion(VerificacionModels vm)
        {

            if (ModelState.IsValid)
            {
                Verificacion updateVerificacion = db.Verificacion.FirstOrDefault(c => c.Id.TrimEnd() == vm.VerificacionUnica.Id);
                updateVerificacion.FechaRegistro = vm.FechaRegistro;
                updateVerificacion.Efectiva = vm.VerificacionUnica.Efectiva;
                updateVerificacion.Descripcion = vm.Descripcion;
                db.Verificacion.Update(updateVerificacion);
                db.SaveChanges();


                if (vm.VerificacionUnica.Efectiva != null)
                {
                    if (vm.VerificacionUnica.Efectiva == true)
                    {

                        Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == vm.EventoId);
                        updateEvento.Estado = 10;

                        db.Evento.Update(updateEvento);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (vm.VerificacionUnica.Efectiva == false)
                        {
                            //verificar si se debe cerrar el evento o abrir uno nuevo con los mismos datos?
                            Evento updateEvento = db.Evento.FirstOrDefault(c => c.Id == vm.EventoId);
                            updateEvento.Estado = 10;

                            db.Evento.Update(updateEvento);
                            db.SaveChanges();

                        }
                    }

                }

                return RedirectToAction("Index", "EventoSecuencia", new { EventoID = vm.EventoId.TrimEnd() });
            }
            else
            {
                return await FormularioEditVerificacion(vm.EventoId.TrimEnd(), vm.VerificacionUnica.Id.TrimEnd());
            }
        }
    }
}
